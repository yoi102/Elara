using Commons.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Commons.Extensions;

public static class FrontendModuleInitializerExtensions
{
    public static IServiceCollection RunFrontendModuleInitializers(this IServiceCollection services,
    IEnumerable<Assembly> assemblies)
    {
        foreach (var asm in assemblies)
        {
            Type[] types = asm.GetTypes();
            var moduleInitializerTypes = types.Where(t => !t.IsAbstract && typeof(IFrontendModuleInitializer).IsAssignableFrom(t));
            foreach (var moduleInitializerType in moduleInitializerTypes)
            {
                var initializer = (IFrontendModuleInitializer?)Activator.CreateInstance(moduleInitializerType);
                if (initializer == null)
                {
                    throw new ApplicationException($"Cannot create ${moduleInitializerType}");
                }
                initializer.Initialize(services);
            }
        }
        return services;
    }
}