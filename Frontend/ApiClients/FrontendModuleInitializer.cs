using ApiClients.Abstractions;
using ApiClients.Abstractions.ChatApiClient.Conversation;
using ApiClients.Abstractions.ChatApiClient.ConversationRequest;
using ApiClients.Abstractions.ChatApiClient.Message;
using ApiClients.Clients;
using ApiClients.Clients.ChatApiClient;
using ApiClients.Clients.PersonalSpaceApiClient;
using Commons.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;

namespace ApiClients;

public class FrontendModuleInitializer : IFrontendModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        var options = new RestClientOptions("https://localhost:8080/Elara")
        {
            ConfigureMessageHandler = handler =>
            {
                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
            }
        };

        var client = new RestClient(options);
        client.AddDefaultHeader("Accept", "application/json");

        services.AddSingleton<IRestClient>(client);
        services.AddSingleton<ITokenRefreshingRestClient, TokenRefreshingRestClient>();
        services.AddTransient<IChatConversationApiClient, ChatConversationApiClient>();
        services.AddTransient<IChatConversationRequestApiClient, ChatConversationRequestApiClient>();
        services.AddTransient<IChatMessageApiClient, ChatMessageApiClient>();
        services.AddTransient<IChatParticipantApiClient, ChatParticipantApiClient>();
        services.AddTransient<IFileApiClient, FileApiClient>();
        services.AddTransient<IPersonalSpaceContactApiClient, PersonalSpaceContactApiClient>();
        services.AddTransient<IPersonalSpaceContactRequestApiClient, PersonalSpaceContactRequestApiClient>();
        services.AddTransient<IPersonalSpaceProfileApiClient, PersonalSpaceProfileApiClient>();
        services.AddTransient<IUserApiClient, UserApiClient>();
        services.AddTransient<IUserIdentityApiClient, UserIdentityApiClient>();
    }
}
