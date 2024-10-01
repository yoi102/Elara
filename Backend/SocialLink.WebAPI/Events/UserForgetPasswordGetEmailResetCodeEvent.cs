using EventBus;
using Microsoft.AspNetCore.Identity.UI.Services;
using SocialLink.Domain.Entities;

namespace SocialLink.WebAPI.Events
{
    [EventName("UserService.User.ResetUserPasswordByEmail")]
    public class ResetUserPasswordByEmailResetCodeEventHandler : JsonIntegrationEventHandler<ResetPasswordByEmailResetCodeEvent>
    {
        private readonly IEmailSender emailSender;
        private readonly ILogger<ResetUserPasswordByEmailResetCodeEventHandler> logger;

        public ResetUserPasswordByEmailResetCodeEventHandler(ILogger<ResetUserPasswordByEmailResetCodeEventHandler> logger, IEmailSender emailSender)
        {
            this.logger = logger;
            this.emailSender = emailSender;
        }

        public override Task HandleJson(string eventName, ResetPasswordByEmailResetCodeEvent eventData)
        {
            logger.LogInformation(eventName, eventData);
            return emailSender.SendEmailAsync(eventData.Email, eventData.Subject, eventData.HtmlMessage);
        }
    }
}