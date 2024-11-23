using DocLink.Domain.DTOs.AuthDtos;
using DocLink.Domain.DTOs.AuthDtos.External_Logins;
using DocLink.Domain.DTOs.AuthDtos.External_Logins.Facebook;
using DocLink.Domain.DTOs.AuthDtos.External_Logins.Google;
using DocLink.Domain.Responses;
using DocLink.Domain.Responses.Genaric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.Interfaces.Services
{
    public interface IAccountService
    {
        Task<BaseResponse<JwtTokenResponse>> RegisterAsync(UserToRegisterDto User);
        Task<BaseResponse<JwtTokenResponse>> LoginAsync(UserToLogInDto User);
        Task<BaseResponse<PasswordResetTokenDto>> ForgetPasswrodAsync(ForgetPasswordDto forgetPassword);
        Task<BaseResponse<bool>> ResetPasswordAsync(ResetPasswordDto resetPassword);
        Task<BaseResponse<JwtTokenResponse>> SignInWithGoogle(GoogleSignInDto Model);
        Task<BaseResponse<JwtTokenResponse>> SignInWithFacebook(FacebookSignInDto model);
		Task<BaseResponse<ConfirmEmailResponse>> ConfirmEmailAsync(ConfirmEmailDto ConfirmEmail);

	}
}
