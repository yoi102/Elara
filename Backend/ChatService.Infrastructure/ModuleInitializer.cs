using ChatService.Domain;
using Commons.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ChatService.Infrastructure;

internal class ModuleInitializer : IBackendModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        services.AddScoped<ChatDomainService>();
        services.AddScoped<IChatServiceRepository, ChatServiceRepository>();
    }
}
