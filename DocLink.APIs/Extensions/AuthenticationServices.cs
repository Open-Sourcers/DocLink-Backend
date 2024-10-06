using System.Text;
using DocLink.Application.Services;
using DocLink.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace DocLink.APIs.Extensions
{
    public static class AuthenticationServices
    {
        public static IServiceCollection AddJwtService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(o =>
             {
                 o.RequireHttpsMetadata = false; // to make it an work at any protocol like http,https
                 o.SaveToken = false;
                 o.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"])),

                     ValidateIssuer = true,
                     ValidIssuer = configuration["JWT:Issuer"],

                     ValidateAudience = true,
                     ValidAudience = configuration["JWT:Audience"],

                     ValidateLifetime = true,
                     ClockSkew = TimeSpan.Zero // To Strict validation of token expiration
                 };
             });
            return services;
        }
    }
}
