using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.DTOs.AuthDtos.External_Logins.Facebook
{
    public class FacebookSignInDto
    {
        [Required]
        public string AccessToken { get; set; }
    }
}
