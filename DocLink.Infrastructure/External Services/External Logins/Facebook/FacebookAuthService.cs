using DocLink.Domain.DTOs.AuthDtos.External_Logins.Facebook;
using DocLink.Domain.DTOs.AuthDtos.External_Logins.Google;
using DocLink.Domain.Entities;
using DocLink.Domain.Enums;
using DocLink.Domain.Interfaces.Services.Exteranl_Logins;
using DocLink.Domain.Responses;
using DocLink.Domain.Responses.FacebookResponses;
using DocLink.Domain.Responses.Genaric;
using DocLink.Infrastructure.Data;
using DocLink.Infrastructure.Extention;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DocLink.Infrastructure.External_Services.External_Logins.Facebook
{
    public class FacebookAuthService : IFacebookAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        private readonly DocLinkContext _context;

        public FacebookAuthService(IHttpClientFactory httpClientFactory,
                                   IConfiguration configuration,
                                   UserManager<AppUser> userManager,
                                   DocLinkContext docLinkContext)
        {
            _httpClient = httpClientFactory.CreateClient("Facebook");
            _configuration = configuration;
            _userManager = userManager;
            _context = docLinkContext;
        }

        public async Task<BaseResponse<AppUser>> FacebookSignInAsync(FacebookSignInDto model)
        {
            var ValidatedFbToken = await ValidateFacebookToken(model.AccessToken);

            if (ValidatedFbToken.Data is null)
                return new BaseResponse<AppUser>(null, "Failed to validate");


            var userInfo = await GetFacebookUserInformation(model.AccessToken);

            if (userInfo.Errors != null && userInfo.Errors.Any())
                return new BaseResponse<AppUser>(null,500,userInfo.Errors);


            var FbUserInfo = userInfo.Data;

            var userToBeCreated = new CreateUserFromSocialLogin
            {
                FirstName = FbUserInfo.FirstName,
                LastName = FbUserInfo.LastName,
                Email = FbUserInfo.Email,
                ProfilePicture = FbUserInfo.Picture.Data.Url.AbsoluteUri,
                LoginProviderSubject = FbUserInfo.Id,
            };

            var user = await _userManager.CreateUserFromSocialLogin(_context, userToBeCreated, LoginProvider.Facebook);

            return new BaseResponse<AppUser>(user);

            //return new BaseResponse(userInfo.Errors); // why this line exist?
        }
        public async Task<BaseResponse<FacebookUserInfoResponse>> GetFacebookUserInformation(string accessToken)
        {
            try
            {
                string userInfoUrl = _configuration["Facebook:UserInfoUrl"];
                string url = string.Format(userInfoUrl, accessToken);

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseAsString = await response.Content.ReadAsStringAsync();
                    var userInfoResponse = JsonConvert.DeserializeObject<FacebookUserInfoResponse>(responseAsString);
                    return new BaseResponse<FacebookUserInfoResponse>(userInfoResponse);
                }
            }
            catch(Exception ex)
            {
                // log your exception using ILog
                //_logger.Error(ex.StackTrace, ex);
            }

            return new BaseResponse<FacebookUserInfoResponse>(null,500,new List<string> { "Failed to get a response." });
        }


        public async Task<BaseResponse<FacebookTokenValidationResponse>> ValidateFacebookToken(string accessToken)
        {
            try
            {
                string TokenValidationUrl = _configuration["Facebook:TokenValidationUrl"];
                var url = string.Format(TokenValidationUrl, accessToken, _configuration["Facebook:AppId"], _configuration["Facebook:AppSecret"]);
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseAsString = await response.Content.ReadAsStringAsync();
                    var tokenValidationResponse = JsonConvert.DeserializeObject<FacebookTokenValidationResponse>(responseAsString);
                    return new BaseResponse<FacebookTokenValidationResponse>(tokenValidationResponse);
                }

            }
            catch (Exception ex)
            {
                // log the exception here
                // _logger.Error(ex.StackTrace, ex);
            }
            return new BaseResponse<FacebookTokenValidationResponse>(null, "Failed to get response");
        }
    }
}
