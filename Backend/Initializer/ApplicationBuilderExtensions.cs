using Microsoft.AspNetCore.Builder;
using EventBus.Extensions;

namespace Initializer
{
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
}