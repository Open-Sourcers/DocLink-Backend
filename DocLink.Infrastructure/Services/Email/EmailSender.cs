using DocLink.Domain.DTOs.ExternalDtos;
using DocLink.Domain.Interfaces.Interfaces;
using DocLink.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System.Formats.Asn1;

namespace DocLink.Infrastructure.Services.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly IEmailService _emailService;
        private readonly string _email;
        public EmailSender(IEmailService emailService, IConfiguration config)
        {
            _emailService = emailService;
            _email = config.GetSection("Email:SenderEmail").Value!;
        }

		public async Task<bool> SendEmailConfirmation(string email, string otp)
		{
            return await _emailService.SendEmail(new EmailDto
            {
                To = email,
                From = _email,
                Subject = "Open Sources - Email Confirmation",
                Body =$"OTP : {otp}"
            }) ;
		}

		public async Task<bool> SendForgetPasswordEmail(string email,string name, string otp)
        {

            return await _emailService.SendEmail(new EmailDto()
            {
                To = email,
                From = _email,
                Subject = "Open Sources - Forget Password",
                Body = $"OTP : {otp}"
            });
        }
    }
}
