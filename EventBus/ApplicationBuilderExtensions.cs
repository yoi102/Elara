using Microsoft.AspNetCore.Builder;

namespace EventBus
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseEventBus(this IApplicationBuilder appBuilder)
        {
            if (appBuilder.ApplicationServices.GetService(typeof(IEventBus)) == null)
            {
                throw new ApplicationException("找不到IEventBus实例");
            }

            return appBuilder;
        }
    }
}
