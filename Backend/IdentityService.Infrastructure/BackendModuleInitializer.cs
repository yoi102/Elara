using Commons.Interfaces;
using IdentityService.Domain;
using IdentityService.Domain.Interfaces;
using IdentityService.Infrastructure.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;


namespace IdentityService.Infrastructure;

internal class BackendModuleInitializer : IBackendModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserDomainService, UserDomainService>();
        services.AddScoped<IEmailSender, EmailSenderServiceMock>();
        services.AddScoped<ICacheService, CacheService>();
    }
}
