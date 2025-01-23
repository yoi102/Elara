using EventBus.Extensions;
using Microsoft.AspNetCore.Builder;

namespace Initializer;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseCommonMiddleware(this IApplicationBuilder app)
    {
        app.UseEventBus();
        app.UseCors();
        app.UseForwardedHeaders();
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }
}