using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocLink.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace DocLink.Domain.Interfaces.Services
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(AppUser user, UserManager<AppUser> userManager);    
    }
}
