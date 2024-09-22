using ASPNETCore.Services;
using Commons.Interfaces;
using DomainCommons.ServiceInterface;
using Microsoft.Extensions.DependencyInjection;

namespace ASPNETCore;

internal class BackendModuleInitializer : IBackendModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddDistributedMemoryCache();
        services.AddScoped<IMemoryCacheService, MemoryCacheService>();
        services.AddScoped<IDistributedCacheService, DistributedCacheService>();
        services.AddScoped<IRedisDbService, RedisDbService>();

    }
}
