using Microsoft.Extensions.DependencyInjection;

namespace Commons.Interfaces;

public interface IBackendModuleInitializer
{
    void Initialize(IServiceCollection services);
}