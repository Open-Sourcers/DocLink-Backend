using DocLink.Domain.DTOs;
using DocLink.Domain.Entities;
using DocLink.Domain.Interfaces.Services;
using DocLink.Domain.Responses;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DocLink.APIs.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IValidator<UserToRegisterDto> registerValidator;
        private readonly IAccountService accountService;
        private readonly UserManager<AppUser> userManager;

        public AccountController(IValidator<UserToRegisterDto> RegisterValidator, IAccountService accountService , UserManager<AppUser> userManager)
        {
            registerValidator = RegisterValidator;
            this.accountService = accountService;
            this.userManager = userManager;
        }
        [HttpPost("Register")]
        public async Task<ActionResult<BaseResponse<UserDto>>> Register(UserToRegisterDto User)
        {
            if (EmailExists(User.Email).Result.Value)
                return BadRequest(new BaseResponse<object>(400, "Email Address is aleardy used"));

            return Ok(await accountService.Register(User));
        }
        [HttpPost("Login")]
        public async Task<ActionResult<BaseResponse<UserDto>>> Login(UserToLogInDto User)
        {
            return await accountService.Login(User);
        }

        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> EmailExists(string Email)
        {
            var user = await userManager.FindByEmailAsync(Email);
            return user is not null;
        }
    }
}
