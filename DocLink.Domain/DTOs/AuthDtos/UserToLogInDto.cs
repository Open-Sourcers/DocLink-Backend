using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.DTOs.AuthDtos
{
    public class UserToLogInDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
