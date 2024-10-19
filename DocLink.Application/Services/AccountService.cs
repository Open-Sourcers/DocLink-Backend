using DocLink.Domain.DTOs.AuthDtos;
using DocLink.Domain.DTOs.AuthDtos.External_Logins;
using DocLink.Domain.DTOs.AuthDtos.External_Logins.Facebook;
using DocLink.Domain.DTOs.AuthDtos.External_Logins.Google;
using DocLink.Domain.Entities;
using DocLink.Domain.Interfaces.Interfaces;
using DocLink.Domain.Interfaces.Services;
using DocLink.Domain.Interfaces.Services.Exteranl_Logins;
using DocLink.Domain.Responses.Genaric;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace DocLink.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IGoogleAuthService _googleAuthService;
        private readonly IFacebookAuthService _facebookAuthService;
        private readonly ICacheService _cache;

		public AccountService(UserManager<AppUser> userManager,
							  ITokenService tokenService,
							  SignInManager<AppUser> signInManager,
							  IEmailSender emailSender,
							  IGoogleAuthService googleAuthService,
							  IFacebookAuthService facebookAuthService,
							  ICacheService cache)
		{
			_userManager = userManager;
			_tokenService = tokenService;
			_signInManager = signInManager;
			_emailSender = emailSender;
			_googleAuthService = googleAuthService;
			_facebookAuthService = facebookAuthService;
			_cache = cache;
		}

		public async Task<BaseResponse<ConfirmEmailResponse>> ConfirmEmailAsync(ConfirmEmailDto confirmEmail)
		{
			var user = await _userManager.FindByEmailAsync(confirmEmail.Email);

			if (user == null)
				return new BaseResponse<ConfirmEmailResponse>("User not Found",StatusCodes.Status404NotFound, new List<string> { "Email not found" });
			
            var cachedOtp = _cache.GetData<string>("Otp");

            if (cachedOtp == null)
				return new BaseResponse<ConfirmEmailResponse>("OTP is Expird",StatusCodes.Status400BadRequest,new List<string> { "Code time expired." });
			

			if (cachedOtp!=confirmEmail.Otp)
				return new BaseResponse<ConfirmEmailResponse>("Invalid OTP ",StatusCodes.Status400BadRequest,new List<string> { "In valid code." });
			
			var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var Data = new ConfirmEmailResponse { Token = token, Email = user.Email };

			return new BaseResponse<ConfirmEmailResponse>(Data, StatusCodes.Status200OK, "Email confirmed successfully.");
		}

		public async Task<BaseResponse<PasswordResetTokenDto>> ForgetPasswrodAsync(ForgetPasswordDto forgetPassword)
        {
            var user = await _userManager.FindByEmailAsync(forgetPassword.Email);
            if (user is null) return new BaseResponse<PasswordResetTokenDto>(null,StatusCodes.Status404NotFound, "Email not found");

            var otp = new Random().Next(100000, 999999).ToString();

            _cache.SetData("Otp",otp,TimeSpan.FromMinutes(10));

            await _emailSender.SendForgetPassword(user.Email,$"{user.FirstName} {user.LastName}", otp);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var response = new PasswordResetTokenDto { Token = token };

            return new BaseResponse<PasswordResetTokenDto>(response , "Check Your Email to get OTP");
        }


        public async Task<BaseResponse<JwtTokenResponse>> LoginAsync(UserToLogInDto User)
        {
            var user = await _userManager.FindByEmailAsync(User.Email);
            if (user is null)
                return new BaseResponse<JwtTokenResponse>(null,StatusCodes.Status400BadRequest, new List<string> { "Email or password is wrong!" });

            var Result = await _signInManager.PasswordSignInAsync(user , User.Password , false , false);

            if (!Result.Succeeded)
                return new BaseResponse<JwtTokenResponse>(null, StatusCodes.Status400BadRequest, new List<string> { "Email or password is wrong!" });

            var token = await _tokenService.GenerateTokenAsync(user, _userManager);
            var data = new JwtTokenResponse { Token = token };
            return new BaseResponse<JwtTokenResponse>(data , "Successfully logged in");
        }


        public async Task<BaseResponse<JwtTokenResponse>> RegisterAsync(UserToRegisterDto User)
        {
            var newUser = new AppUser()
            {
                FirstName = User.FirstName,
                LastName = User.LastName,
                Email = User.Email,
                UserName = User.Email.Split('@')[0]
            };

            var Result = await _userManager.CreateAsync(newUser, User.Password);

            if (!Result.Succeeded)
            {
                var errors = Result.Errors.Select(E => E.Description).ToList(); // try to use identityResult.Errors it self ?
                var Response = new BaseResponse<JwtTokenResponse>(null,StatusCodes.Status500InternalServerError,errors);
                return Response;
            }

            var token = await _tokenService.GenerateTokenAsync(newUser, _userManager);
            var data = new JwtTokenResponse { Token = token };
			return new BaseResponse<JwtTokenResponse>(data, "Registeration done successfully");
		}

        public async Task<BaseResponse<bool>> ResetPasswordAsync(ResetPasswordDto resetPassword)
        {
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);

            if(user is null) return new BaseResponse<bool>("Faild to Find Email",StatusCodes.Status400BadRequest, new List<string> {"Your Email Not Found" });

            var identityResult = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.NewPassword);

            if(!identityResult.Succeeded)
            {
                var Errors = identityResult.Errors.ToString();
                return new BaseResponse<bool>(false,"Falid To Reset Password" , new List<string> { Errors } , StatusCodes.Status500InternalServerError);
            }

            _cache.RemoveData("Otp");

            return new BaseResponse<bool>(true,StatusCodes.Status200OK, "Password has updated Successfully.");
        }



        public async Task<BaseResponse<JwtTokenResponse>> SignInWithFacebook(FacebookSignInDto model)
        {

            var user = await _facebookAuthService.FacebookSignInAsync(model);

            if (user is null) return new BaseResponse<JwtTokenResponse>();

            var jwtResponse = await _tokenService.GenerateTokenAsync(user.Data, _userManager);

            var Data = new JwtTokenResponse { Token = jwtResponse };

           return new BaseResponse<JwtTokenResponse>(Data);
        }


        public async Task<BaseResponse<JwtTokenResponse>> SignInWithGoogle(GoogleSignInDto Model)
        {
            var Response = await _googleAuthService.GoogleSignInAsync(Model);
            if (Response.Errors.Any())
                return new BaseResponse<JwtTokenResponse>(Response.ResponseMessage,Response.StatusCode ,Response.Errors);

            
            var token = await _tokenService.GenerateTokenAsync(Response.Data, _userManager);
            var data = new JwtTokenResponse { Token = token };

            return new BaseResponse<JwtTokenResponse>(data);
        }
    }
}
