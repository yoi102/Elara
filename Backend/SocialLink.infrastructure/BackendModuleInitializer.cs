using Commons.Interfaces;
using JWT;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SocialLink.Domain.Interfaces;
using SocialLink.Domain.Services;
using SocialLink.infrastructure.Services;

namespace SocialLink.infrastructure
{
    internal class BackendModuleInitializer : IBackendModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserDomainService, UserDomainService>();
            services.AddScoped<IEmailSender, EmailSenderServiceMock>();
            services.AddSingleton<IEmailResetCodeValidator, EmailResetCodeValidatorService>();
        }
    }
}