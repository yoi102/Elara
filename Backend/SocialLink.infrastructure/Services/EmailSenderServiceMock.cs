using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLink.infrastructure.Services
{
    public class EmailSenderServiceMock : IEmailSender
    {
        private readonly ILogger<EmailSenderServiceMock> logger;

        public EmailSenderServiceMock(ILogger<EmailSenderServiceMock> logger)
        {
            this.logger = logger;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            logger.LogInformation("Send Email to {0},title:{1}, body:{2}", email, subject, htmlMessage);
            //https://api.XXXX/api/mail/send
            return Task.CompletedTask;

        }
    }
}
