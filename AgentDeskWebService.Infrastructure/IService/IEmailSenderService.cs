using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Infrastructure.IService
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(string subject, string htmlBody, string toEmail, int maxRetries = 3);
    }
}
