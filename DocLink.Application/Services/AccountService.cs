
using DocLink.Domain.DTOs;
using DocLink.Domain.Entities;
using DocLink.Domain.Interfaces.Services;
using DocLink.Domain.Responses;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public AccountService(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }
        public async Task<BaseResponse<UserDto>> Register(UserToRegisterDto User)
        {
            var newUser = new AppUser()
            {
                FirstName = User.FirstName,
                LastName = User.LastName,
                Email = User.Email,
                UserName = User.Email.Split('@')[0]
            };

            var Result = await _userManager.CreateAsync(newUser , User.Password);
           
            if(!Result.Succeeded)
            {
                var errors = Result.Errors.Select(E => E.Description).ToList();
                var Response = new BaseResponse<UserDto>(errors);
                return Response;
            }

            var token = await _tokenService.GenerateTokenAsync(newUser, _userManager);
            var ReturnUser = new UserDto()
            {
                DisplayName = User.FirstName + ' ' + User.LastName,
                Email = User.Email,
                Token = token
            };
           return new BaseResponse<UserDto>(ReturnUser);
        }

      
    }
}
