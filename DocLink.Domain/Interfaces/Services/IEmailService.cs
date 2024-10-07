using DocLink.Domain.DTOs.ExternalDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.Interfaces.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmail(EmailDto email);

    }
}
