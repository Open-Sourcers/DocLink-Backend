using DocLink.Domain.DTOs.ExternalDtos;
using DocLink.Domain.Interfaces.Interfaces;
using DocLink.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace DocLink.Infrastructure.Services.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly IEmailService _emailService;
        private readonly string _email;
        private readonly IWebHostEnvironment _webhost;
		public EmailSender(IEmailService emailService, IConfiguration config, IWebHostEnvironment webhost)
		{
			_emailService = emailService;
			_email = config.GetSection("Email:SenderEmail").Value!;
			_webhost = webhost;
		}

		public async Task<bool> SendEmailConfirmation(string email, string otp)
		{
			return await _emailService.SendEmail(new EmailDto
			{
				To = email,
				From = _email,
				Subject = "Open Sources - Email Confirmation",
				Body = $"OTP : {otp}"
			});
		}

		public async Task<bool> SendForgetPassword(string email,string name, string otp)
        {

            // var x=File.ReadAllText(_webhost.WebRootPath)

            return await _emailService.SendEmail(new EmailDto()
            {
                To = email,
                From = _email,
                Subject = "Open Sources - Forget Password",
                Body = $"OTP: {otp}",
            });
        }
    }
}
