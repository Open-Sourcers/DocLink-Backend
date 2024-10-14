using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.Interfaces.Interfaces
{
    public interface IEmailSender
    {
        Task<bool> SendForgetPassword(string email,string name, string otp);
		Task<bool> SendEmailConfirmation(string email, string otp);

	}
}
