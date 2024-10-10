﻿using DocLink.Domain.DTOs.AuthDtos.External_Logins.Google;
using DocLink.Domain.Entities;
using DocLink.Domain.Enums;
using DocLink.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;


namespace DocLink.Infrastructure.Extention
{
    public static class CreateUserFromSocialLoginExtension
    {
        public static async Task<AppUser> CreateUserFromSocialLogin(this UserManager<AppUser> userManager ,
                    DocLinkContext docLinkContext , CreateUserFromSocialLogin Model , LoginProvider loginProvider ) 
        {
            var user = await userManager.FindByLoginAsync(loginProvider.GetDisplayName() , Model.LoginProviderSubject);
            if (user is not null)
                return user;

            user = await userManager.FindByEmailAsync(Model.Email);

            if(user is null)
            {
                user = new AppUser
                {
                    FirstName = Model.FirstName,
                    LastName = Model.LastName,
                    Email = Model.Email,
                    UserName = Model.Email.Split("@")[0],
                    ProfilePecture = Model.ProfilePicture
                };

                await userManager.CreateAsync(user);

                //if email confirmed
                //user.EmailConfirmed = true;

                await userManager.UpdateAsync(user);
                await docLinkContext.SaveChangesAsync();
            }


            UserLoginInfo userLoginInfo = null;

            switch (loginProvider)
            {
                case LoginProvider.Google:
                    {
                        userLoginInfo = new UserLoginInfo(loginProvider.GetDisplayName() ,Model.LoginProviderSubject , loginProvider.GetDisplayName().ToUpper());
                    }
                    break;
                case LoginProvider.Facebook:
                    break;
                default:
                    break;
            }

            var result = await userManager.AddLoginAsync(user, userLoginInfo);

            if (result.Succeeded)
                return user;

            return null;
        }

        
        
    }
}