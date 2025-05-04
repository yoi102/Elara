using ApiClients.Abstractions.ChatApiClient;
using ApiClients.Abstractions.FileApiClient;
using ApiClients.Abstractions.PersonalSpaceApiClient.Profile;
using ApiClients.Abstractions.UserApiClient;
using ApiClients.Abstractions.UserIdentityApiClient;
using ApiClients.Clients;
using Commons.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;

namespace ApiClients;

public class FrontendModuleInitializer : IFrontendModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        var client = new RestClient("http://localhost:8080/Elara");
        client.AddDefaultHeader("Accept", "application/json");

        services.AddSingleton<IRestClient>(client);
        services.AddSingleton<ITokenRefreshingRestClient, TokenRefreshingRestClient>();
        services.AddTransient<IUserApiClient, UserApiClient>();
        services.AddTransient<IUserIdentityApiClient, UserIdentityApiClient>();
        services.AddTransient<IPersonalSpaceProfileApiClient, PersonalSpaceProfileApiClient>();
        services.AddTransient<IFileApiClient, FileApiClient>();
        services.AddTransient<IChatApiClient, ChatApiClient>();
    }
}
