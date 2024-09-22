using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventBus.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseEventBus(this IApplicationBuilder appBuilder)
    {
        if (appBuilder.ApplicationServices.GetService<IEventBus>() == null)
        {
            throw new ApplicationException("IEventBus instance not found");
        }

        return appBuilder;
    }
}