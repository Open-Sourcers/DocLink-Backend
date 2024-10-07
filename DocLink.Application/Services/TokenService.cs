using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DocLink.Domain.Entities;
using DocLink.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DocLink.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> GenerateTokenAsync(AppUser user, UserManager<AppUser> userManager)
        {
            var userClaims = new List<Claim>
            { 
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.FirstName + ' ' + user.LastName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var Roles =  await userManager.GetRolesAsync(user);

            foreach (var Role in Roles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, Role));
            }

            var authKeyInByets = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            var JwtObject = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:ExpiryDays"])),
                signingCredentials: new SigningCredentials(authKeyInByets, SecurityAlgorithms.HmacSha256Signature)
            );
            return new JwtSecurityTokenHandler().WriteToken(JwtObject);
        }
    }
}
