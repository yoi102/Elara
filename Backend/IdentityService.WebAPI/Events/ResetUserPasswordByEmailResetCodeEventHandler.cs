using EventBus.Attributes;
using EventBus.Handlers;
using IdentityService.WebAPI.Events.Args;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace IdentityService.WebAPI.Events
{
    [EventName("UserService.User.ResetUserPasswordByEmail")]
    public class ResetUserPasswordByEmailResetCodeEventHandler : JsonIntegrationEventHandler<ResetPasswordByEmailResetCodeEventArgs>
    {
        private readonly IEmailSender emailSender;
        private readonly ILogger<ResetUserPasswordByEmailResetCodeEventHandler> logger;

        public ResetUserPasswordByEmailResetCodeEventHandler(ILogger<ResetUserPasswordByEmailResetCodeEventHandler> logger, IEmailSender emailSender)
        {
            this.logger = logger;
            this.emailSender = emailSender;
        }

        public override Task HandleJson(string eventName, ResetPasswordByEmailResetCodeEventArgs eventData)
        {
            logger.LogInformation(eventName, eventData);
            return emailSender.SendEmailAsync(eventData.Email, eventData.Subject, eventData.HtmlMessage);
        }
    }
}