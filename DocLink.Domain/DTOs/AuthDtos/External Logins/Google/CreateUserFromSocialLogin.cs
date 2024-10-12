using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.DTOs.AuthDtos.External_Logins.Google
{
    public class CreateUserFromSocialLogin
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ProfilePicture { get; set; }
        public string LoginProviderSubject { get; set; }
    }
}
