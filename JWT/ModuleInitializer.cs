using Commons;
using Microsoft.Extensions.DependencyInjection;

namespace JWT
{
    internal class ModuleInitializer : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
        }
    }
}