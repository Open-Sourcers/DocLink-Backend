using DocLink.Domain.DTOs;
using DocLink.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.Interfaces.Services
{
    public interface IAccountService
    {
        // Register (UserToRegistrDto) => UserDto
        Task<BaseResponse<UserDto>> Register(UserToRegisterDto User);
        Task<BaseResponse<UserDto>> Login(UserToLogInDto User);

        // login (UserToLoginDto) =>
    }
}
