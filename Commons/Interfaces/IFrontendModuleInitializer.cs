using Microsoft.Extensions.DependencyInjection;

namespace Commons.Interfaces;

public interface IFrontendModuleInitializer
{
    void Initialize(IServiceCollection services);
}