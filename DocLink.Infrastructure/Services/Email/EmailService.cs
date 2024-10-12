using DocLink.Domain.DTOs.ExternalDtos;
using DocLink.Domain.Interfaces.Services;
using FluentEmail.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Infrastructure.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly IFluentEmail _fluentEmail;

        public EmailService(IFluentEmail fluentEmail)
        {
            _fluentEmail = fluentEmail;
        }

        public async Task<bool> SendEmail(EmailDto email)
        {
            var request = await _fluentEmail.To(email.To)
                .Subject(email.Subject)
                .Body(email.Body)
                .SendAsync();
            return request.Successful; 
        }
    }
}
