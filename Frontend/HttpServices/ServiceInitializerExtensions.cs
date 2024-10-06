using Microsoft.Extensions.DependencyInjection;

namespace HttpServices
{
    public static class ServiceInitializerExtensions
    {
        public static void InitializeHttpServices(this IServiceCollection service)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:8080/Elara/")
            };
            service.AddSingleton(httpClient);

        }
    }
}
