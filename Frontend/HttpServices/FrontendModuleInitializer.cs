using Commons.Interfaces;
using HttpServices.Services;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using Service.Abstractions;

namespace HttpServices
{
    public class FrontendModuleInitializer : IFrontendModuleInitializer
    {


        public void Initialize(IServiceCollection services)
        {

            var client = new RestClient("http://localhost:8080/Elara");
            client.AddDefaultHeader("Accept", "application/json");

            services.AddSingleton(client);
            services.AddTransient<IUserService, UserService>();
        }
    }
}
