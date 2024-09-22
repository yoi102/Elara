using Commons.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions;

namespace Services;

public class FrontendModuleInitializer : IFrontendModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        services.AddTransient<IUserIdentityService, UserIdentityService>();
    }
}
