using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.DTOs.AuthDtos
{
    public class ConfirmEmailResponse
    {
        public string Token { get; set; }
        public string Email { get; set; }
    }
}
