using Commons.Interfaces;
using DataProviders.Abstractions;
using DataProviders.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace DataProviders;

public class FrontendModuleInitializer : IFrontendModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        services.AddSingleton<IUserDataProvider, UserDataProvider>();
        services.AddTransient<IUserAgentProvider, UserAgentProvider>();
    }
}
