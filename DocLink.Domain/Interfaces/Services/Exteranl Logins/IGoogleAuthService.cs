using DocLink.Domain.DTOs.AuthDtos.External_Logins.Google;
using DocLink.Domain.Entities;
using DocLink.Domain.Responses;
using DocLink.Domain.Responses.Genaric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.Interfaces.Services.Exteranl_Logins
{
    public interface IGoogleAuthService
    {
        Task<BaseResponse<AppUser>> GoogleSignInAsync(GoogleSignInDto Model);
    }
}
