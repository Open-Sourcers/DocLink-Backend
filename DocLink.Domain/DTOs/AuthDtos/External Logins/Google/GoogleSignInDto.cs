using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.DTOs.AuthDtos.External_Logins.Google
{
    public class GoogleSignInDto
    {
        [Required]
        public string IdToken { get; set; }
    }
}
