using Commons.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ASPNETCore
{
    internal class BackendModuleInitializer : IBackendModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddScoped<IMemoryCacheHelper, MemoryCacheHelper>();
            services.AddScoped<IDistributedCacheHelper, DistributedCacheHelper>();

        }
    }
}