using Commons.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HttpServices
{
    public class FrontendModuleInitializer : IFrontendModuleInitializer
    {


        public void Initialize(IServiceCollection services)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:8080/Elara/")
            };
            services.AddSingleton(httpClient);
        }
    }
}
