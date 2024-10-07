using DocLink.Domain.DTOs.AuthDtos;
using DocLink.Domain.Entities;
using DocLink.Domain.Interfaces.Interfaces;
using DocLink.Domain.Interfaces.Services;
using DocLink.Domain.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
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

        public AccountService(UserManager<AppUser> userManager,
                              ITokenService tokenService,
                              SignInManager<AppUser> signInManager,
                              IMemoryCache memoryCache,
                              IEmailSender emailSender)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _memoryCache = memoryCache;
            _emailSender = emailSender;
        }

        public async Task<BaseResponse> ForgetPasswrodAsync(ForgetPasswordDto forgetPassword)
        {
            var user = await _userManager.FindByEmailAsync(forgetPassword.Email);
            if (user is null) return new BaseResponse(StatusCodes.Status404NotFound, "Email not found");
            var otp = new Random().Next(100000, 999999).ToString();
            _memoryCache.Set(user.Email, otp, TimeSpan.FromMinutes(10));

            // send email to this email with otp code
            await _emailSender.SendEmailConfirmationAsync(user.Email, otp);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return new BaseResponse(token, _massage: $"please confirm your email your otp {otp}");
        }

        public async Task<BaseResponse> LoginAsync(UserToLogInDto User)
        {
            var user = await _userManager.FindByEmailAsync(User.Email);
            if (user is null)
                return new BaseResponse(StatusCodes.Status404NotFound);
            var Result = await _signInManager.CheckPasswordSignInAsync(user, User.Password, false);

            if (!Result.Succeeded)
                return new BaseResponse(StatusCodes.Status400BadRequest, "email or password is wrong!");

            var token = await _tokenService.GenerateTokenAsync(user, _userManager);
            var ReturndUser = new UserDto
            {
                DisplayName = user.FirstName + ' ' + user.LastName,
                Email = User.Email,
                Token = token,
            };
            return new BaseResponse(ReturndUser);
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
                var errors = Result.Errors.Select(E => E.Description).ToList(); // try to use Result.Errors it self ?
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
            return new BaseResponse(ReturnUser);
        }

        public async Task<BaseResponse> ResetPasswordAsync(ResetPasswordDto resetPassword)
        {
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if(user is null) return new BaseResponse(StatusCodes.Status401Unauthorized);

            if (!_memoryCache.TryGetValue(resetPassword.Email, out string Otp))
                return new BaseResponse(StatusCodes.Status400BadRequest, "Time Expired please try again");

            if(Otp != resetPassword.Otp)
                return new BaseResponse(StatusCodes.Status400BadRequest, "Invalid OTP Code");

            var Result = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.NewPassword);
            if(!Result.Succeeded)
            {
                var Errors = Result.Errors;
                return new BaseResponse(Errors.ToList(), StatusCodes.Status500InternalServerError);
            }

            return new BaseResponse(StatusCodes.Status200OK, _massage: "Password has updated Successfully.");
        }
    }
}
