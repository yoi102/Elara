using Commons.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace JWT;

internal class BackendModuleInitializer : IBackendModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
    }
}