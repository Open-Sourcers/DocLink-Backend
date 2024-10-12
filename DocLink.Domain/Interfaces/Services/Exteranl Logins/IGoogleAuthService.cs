using DocLink.Domain.DTOs.AuthDtos.External_Logins.Google;
using DocLink.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.Interfaces.Services.Exteranl_Logins
{
    public interface IGoogleAuthService
    {
        Task<BaseResponse> GoogleSignInAsync(GoogleSignInDto Model);
    }
}
