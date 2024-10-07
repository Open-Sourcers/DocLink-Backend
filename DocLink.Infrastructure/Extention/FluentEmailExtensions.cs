using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Mail;
using System.Net;

namespace DocLink.Infrastructure.Extention
{
    public static class FluentEmailExtensions
    {
        public static IServiceCollection AddFluentEmailServices(this IServiceCollection services, IConfiguration configuration)
        {
            var emailSettings = configuration.GetSection("Email");

            var userName = emailSettings["UserName"];
            var password = emailSettings["Password"];
            var networkCredential=new NetworkCredential(userName, password);


            var enableSsl = emailSettings.GetValue<bool>("EnableSSL");
            var host = emailSettings["Server"];
            var defaultFromEmail = emailSettings["SenderEmail"];
            var useDefaultCredentials = emailSettings.GetValue<bool>("UseDefaultCredentials");
            var port = emailSettings.GetValue<int>("Port");

            var smtpClient = new SmtpClient
            {
                EnableSsl = enableSsl,
                Host = host!,
                Port = port,
                UseDefaultCredentials = useDefaultCredentials,
                Credentials = networkCredential,
            };
            services.AddFluentEmail(defaultFromEmail)
                .AddSmtpSender(smtpClient);

            return services;
        }
    }
}
#region smtpClient

#endregion
