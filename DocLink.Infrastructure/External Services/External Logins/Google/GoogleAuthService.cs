using DocLink.Domain.DTOs.AuthDtos.External_Logins.Google;
using DocLink.Domain.Entities;
using DocLink.Domain.Enums;
using DocLink.Domain.Interfaces.Services.Exteranl_Logins;
using DocLink.Domain.Responses;
using DocLink.Infrastructure.Data;
using DocLink.Infrastructure.Extention;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace DocLink.Infrastructure.External_Services.External_Logins.Google
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly DocLinkContext _docLinkContext;
        private readonly IConfiguration _configuration;

        public GoogleAuthService(UserManager<AppUser> userManager , DocLinkContext docLinkContext , IConfiguration configuration)
        {
            this._userManager = userManager;
            this._docLinkContext = docLinkContext;
            this._configuration = configuration;
        }
        public async Task<BaseResponse> GoogleSignInAsync(GoogleSignInDto Model)
        {
            Payload payload = new();

            try 
            {
                payload = await ValidateAsync(Model.IdToken, new ValidationSettings
                {
                    Audience = new[] { _configuration["Google:ClientId"]}
                });    
            
            }
            catch(Exception ex)
            {
                // logger here 
                //_logger.Error(ex.Message, ex);
                return new BaseResponse(new List<string> { "Failed to get a response." });
            }

            var UserToBeCreated = new CreateUserFromSocialLogin
            {
               FirstName = payload.GivenName,
               LastName = payload.FamilyName,
               Email = payload.Email,
               ProfilePicture = payload.Picture
            };

            var User = _userManager.CreateUserFromSocialLogin(_docLinkContext , UserToBeCreated , LoginProvider.Google);

            if (User is not null)
                return new BaseResponse(User);

            else
                return new BaseResponse(new List<string> { "Unable to link a Local User to a Provider" });
        }
    }
}
