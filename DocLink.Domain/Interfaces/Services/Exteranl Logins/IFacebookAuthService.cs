using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocLink.Domain.DTOs.AuthDtos.External_Logins.Facebook;
using DocLink.Domain.Entities;
using DocLink.Domain.Responses;
using DocLink.Domain.Responses.FacebookResponses;
using DocLink.Domain.Responses.Genaric;

namespace DocLink.Domain.Interfaces.Services.Exteranl_Logins
{
    public interface IFacebookAuthService
    {
        Task<BaseResponse<AppUser>> FacebookSignInAsync(FacebookSignInDto model);
        Task<BaseResponse<FacebookTokenValidationResponse>> ValidateFacebookToken(string accessToken);
        Task<BaseResponse<FacebookUserInfoResponse>> GetFacebookUserInformation(string accessToken);
    }
}
