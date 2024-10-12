using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocLink.Domain.DTOs.AuthDtos.External_Logins.Facebook;
using DocLink.Domain.Responses;

namespace DocLink.Domain.Interfaces.Services.Exteranl_Logins
{
    public interface IFacebookAuthService
    {
        Task<BaseResponse> FacebookSignInAsync(FacebookSignInDto model);
        Task<BaseResponse> ValidateFacebookToken(string accessToken);
        Task<BaseResponse> GetFacebookUserInformation(string accessToken);
    }
}
