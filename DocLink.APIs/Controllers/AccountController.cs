using DocLink.APIs.Validators;
using DocLink.Domain.DTOs.AuthDtos;
using DocLink.Domain.DTOs.AuthDtos.External_Logins;
using DocLink.Domain.DTOs.AuthDtos.External_Logins.Facebook;
using DocLink.Domain.DTOs.AuthDtos.External_Logins.Google;
using DocLink.Domain.Entities;
using DocLink.Domain.Interfaces.Services;
using DocLink.Domain.Responses;
using DocLink.Domain.Responses.Genaric;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace DocLink.APIs.Controllers
{
    public class AccountController : BaseController
    {
       // private readonly IValidator<ConfirmEmailDto> _confirmEmailValidator;
        private readonly IValidator<UserToRegisterDto> _registerValidator;
        private readonly IAccountService _accountService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IValidator<UserToLogInDto> _userToLoginValitor;

        public AccountController(IValidator<UserToRegisterDto> RegisterValidator,
                                 IAccountService accountService,
                                 UserManager<AppUser> userManager,
                                 IValidator<UserToLogInDto> UserToLoginValitor)
        {
            _registerValidator = RegisterValidator;
            _accountService = accountService;
            _userManager = userManager;
            _userToLoginValitor = UserToLoginValitor;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserToRegisterDto User)
        {
            if (EmailExists(User.Email).Result.Value)
                return BadRequest(new BaseResponse<JwtTokenResponse>(null , StatusCodes.Status400BadRequest , new List<string> {"Email Address is used before"}));

            var result = await _accountService.RegisterAsync(User);

            if(result.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError,result);

            return Ok(result);
        }


        [HttpPost("Login")]
        public async Task<ActionResult<BaseResponse<JwtTokenResponse>>> Login(UserToLogInDto User)
        {
            var result =  await _accountService.LoginAsync(User);
            if (result.StatusCode == StatusCodes.Status400BadRequest)
                return BadRequest(result);
            
            return Ok(result);
        }



        [HttpPost("forget-password")]
        public async Task<ActionResult<BaseResponse>> ForgetPassword(ForgetPasswordDto forgetPasswordDto)
        {
            var Result = await _accountService.ForgetPasswrodAsync(forgetPasswordDto);

            if (Result.StatusCode == StatusCodes.Status404NotFound)
                return NotFound(Result);

            return Ok(Result);
        }



        [HttpPost("Reset-Password")]
        public async Task<ActionResult<BaseResponse<bool>>> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var Result = await _accountService.ResetPasswordAsync(resetPasswordDto);

            if (Result.StatusCode == StatusCodes.Status400BadRequest)
                return BadRequest(Result);
            else if (Result.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(500, Result);

            return Ok(Result);
        }


        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> EmailExists(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            return user is not null;
        }



        [HttpPost("SignIn-Google")]
        public async Task<ActionResult<BaseResponse<JwtTokenResponse>>> SignInWithGoogle(GoogleSignInDto Model)
        {
            var Result = await _accountService.SignInWithGoogle(Model);

            if (Result.StatusCode == StatusCodes.Status401Unauthorized)
                return Unauthorized(Result);

            else if (Result.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, Result);

            return Ok(Result);
        }

        [HttpPost("SignIn-Facebook")]
        public async Task<ActionResult<BaseResponse>> SignInWithFacebook(FacebookSignInDto model)
        {
            var Result = await _accountService.SignInWithFacebook(model);

            return Ok(Result);
        }

        [HttpPost("Confirm-Email")]
        public async Task<ActionResult<BaseResponse>> ConfirmEmail(ConfirmEmailDto model)
        {
            return Ok(await _accountService.ConfirmEmailAsync(model));
        }
    }
}
