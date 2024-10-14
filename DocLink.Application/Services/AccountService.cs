using DocLink.Domain.DTOs.AuthDtos;
using DocLink.Domain.DTOs.AuthDtos.External_Logins.Facebook;
using DocLink.Domain.DTOs.AuthDtos.External_Logins.Google;
using DocLink.Domain.Entities;
using DocLink.Domain.Enums;
using DocLink.Domain.Interfaces.Interfaces;
using DocLink.Domain.Interfaces.Services;
using DocLink.Domain.Interfaces.Services.Exteranl_Logins;
using DocLink.Domain.Responses;
using DocLink.Domain.Responses.FacebookResponses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMemoryCache _memoryCache;
        private readonly IEmailSender _emailSender;
        private readonly IGoogleAuthService _googleAuthService;
        private readonly IFacebookAuthService _facebookAuthService;
        private readonly ICacheService _cache;

		public AccountService(UserManager<AppUser> userManager,
							  ITokenService tokenService,
							  SignInManager<AppUser> signInManager,
							  IMemoryCache memoryCache,
							  IEmailSender emailSender,
							  IGoogleAuthService googleAuthService,
							  IFacebookAuthService facebookAuthService,
							  ICacheService cache)
		{
			_userManager = userManager;
			_tokenService = tokenService;
			_signInManager = signInManager;
			_memoryCache = memoryCache;
			_emailSender = emailSender;
			_googleAuthService = googleAuthService;
			_facebookAuthService = facebookAuthService;
			_cache = cache;
		}

		public async Task<BaseResponse> ConfirmEmailAsync(ConfirmEmailDto confirmEmail)
		{
			var user = await _userManager.FindByEmailAsync(confirmEmail.Email);
			if (user == null)
			{
				return new BaseResponse(StatusCodes.Status404NotFound, "User not found!");
			}
            var cachedOtp = _cache.GetData<string>("Otp");
            if (cachedOtp == null)
            {
				return new BaseResponse(StatusCodes.Status400BadRequest, "Code time expired.");
			}

			if (cachedOtp!=confirmEmail.Otp)
			{
				return new BaseResponse(StatusCodes.Status400BadRequest, "In valid code.");
			}

			var token = await _userManager.GeneratePasswordResetTokenAsync(user);
			return new BaseResponse(new{ Token=token,Email=user.Email},StatusCodes.Status200OK, "Email confirmed successfully.");
		}

		public async Task<BaseResponse> ForgetPasswrodAsync(ForgetPasswordDto forgetPassword)
        {
            var user = await _userManager.FindByEmailAsync(forgetPassword.Email);
            if (user is null) return new BaseResponse(StatusCodes.Status404NotFound, "Email not found");

            var otp = new Random().Next(100000, 999999).ToString();
            _cache.SetData("Otp",otp,TimeSpan.FromMinutes(10));

            await _emailSender.SendForgetPassword(user.Email,$"{user.FirstName} {user.LastName}", otp);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return new BaseResponse(token, _massage: $"please confirm your email your otp {otp}");
        }

        public async Task<BaseResponse> LoginAsync(UserToLogInDto User)
        {
            var user = await _userManager.FindByEmailAsync(User.Email);
            if (user is null)
                return new BaseResponse(StatusCodes.Status400BadRequest, "Email or password is wrong!");

            var Result = await _signInManager.CheckPasswordSignInAsync(user, User.Password, false);

            if (!Result.Succeeded)
                return new BaseResponse(StatusCodes.Status400BadRequest, "Email or password is wrong!");

            var token = await _tokenService.GenerateTokenAsync(user, _userManager);
            var ReturndUser = new UserDto
            {
                DisplayName = user.FirstName + ' ' + user.LastName,
                Email = User.Email,
                Token = token,
            };
            return new BaseResponse(ReturndUser , 200 , "Successfully logged in");
        }

        public async Task<BaseResponse> RegisterAsync(UserToRegisterDto User)
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
                var Response = new BaseResponse(errors);
                return Response;
            }

            var token = await _tokenService.GenerateTokenAsync(newUser, _userManager);
            var ReturnUser = new UserDto()
            {
                DisplayName = User.FirstName + ' ' + User.LastName,
                Email = User.Email,
                Token = token
            };

			return new BaseResponse(ReturnUser,200, "Registration successful.");
		}

        public async Task<BaseResponse> ResetPasswordAsync(ResetPasswordDto resetPassword)
        {
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if(user is null) return new BaseResponse(StatusCodes.Status401Unauthorized);

            var identityResult = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.NewPassword);
            if(!identityResult.Succeeded)
            {
                var Errors = identityResult.Errors;
                return new BaseResponse(Errors.ToList(), StatusCodes.Status500InternalServerError);
            }

            return new BaseResponse(StatusCodes.Status200OK, _massage: "Password has updated Successfully.");
        }

        public async Task<BaseResponse> SignInWithFacebook(FacebookSignInDto model)
        {

            var user = await _facebookAuthService.FacebookSignInAsync(model);

            if (user is null) return new BaseResponse(StatusCodes.Status400BadRequest);

            var jwtResponse = await _tokenService.GenerateTokenAsync((AppUser) user.Data, _userManager);

           return new BaseResponse(jwtResponse);
        }

        public async Task<BaseResponse> SignInWithGoogle(GoogleSignInDto Model)
        {
            var Response = await _googleAuthService.GoogleSignInAsync(Model);
            if (Response.Data is null) return new BaseResponse(StatusCodes.Status400BadRequest);

            var appUser = (AppUser)Response.Data;
            var token = await _tokenService.GenerateTokenAsync(appUser, _userManager);
            var ReturnUser = new UserDto
            {
                DisplayName = appUser.FirstName + ' ' + appUser.LastName,
                Email = appUser.Email,
                Token = token
            };
            return new BaseResponse(ReturnUser);
        }
    }
}
