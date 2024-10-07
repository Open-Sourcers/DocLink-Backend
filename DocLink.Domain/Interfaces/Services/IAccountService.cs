using DocLink.Domain.DTOs.AuthDtos;
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
        Task<BaseResponse> RegisterAsync(UserToRegisterDto User);
        // login (UserToLoginDto) =>
        Task<BaseResponse> LoginAsync(UserToLogInDto User);
        Task<BaseResponse> ForgetPasswrodAsync(ForgetPasswordDto forgetPassword);

        Task<BaseResponse> ResetPasswordAsync(ResetPasswordDto resetPassword);
    }
}
