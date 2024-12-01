using Commons.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using PersonalSpaceService.Domain;
using PersonalSpaceService.Domain.Interfaces;
using PersonalSpaceService.Infrastructure.Configs;

namespace PersonalSpaceService.Infrastructure
{
    internal class BackendModuleInitializer : IBackendModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddScoped<IPersonalSpaceRepository, PersonalSpaceRepository>();
            services.AddScoped<PersonalSpaceDomain>();
        }
    }
}
