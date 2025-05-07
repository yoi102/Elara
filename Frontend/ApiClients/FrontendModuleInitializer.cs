using ApiClients.Abstractions.ChatApiClient.Conversation;
using ApiClients.Abstractions.ChatApiClient.ConversationRequest;
using ApiClients.Abstractions.ChatApiClient.Message;
using ApiClients.Abstractions.ChatApiClient.Participant;
using ApiClients.Abstractions.FileApiClient;
using ApiClients.Abstractions.PersonalSpaceApiClient.ContactRequest;
using ApiClients.Abstractions.PersonalSpaceApiClient.Profile;
using ApiClients.Abstractions.UserApiClient;
using ApiClients.Abstractions.UserIdentityApiClient;
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
        var client = new RestClient("http://localhost:8080/Elara");
        client.AddDefaultHeader("Accept", "application/json");

        services.AddSingleton<IRestClient>(client);
        services.AddSingleton<ITokenRefreshingRestClient, TokenRefreshingRestClient>();
        services.AddTransient<IChatConversationApiClient, ChatConversationApiClient>();
        services.AddTransient<IChatConversationRequestApiClient, ChatConversationRequestApiClient>();
        services.AddTransient<IChatMessageApiClient, ChatMessageApiClient>();
        services.AddTransient<IChatParticipantApiClient, ChatParticipantApiClient>();
        services.AddTransient<IFileApiClient, FileApiClient>();
        services.AddTransient<IPersonalSpaceContactRequestApiClient, PersonalSpaceContactRequestApiClient>();
        services.AddTransient<IPersonalSpaceProfileApiClient, PersonalSpaceProfileApiClient>();
        services.AddTransient<IPersonalSpaceProfileApiClient, PersonalSpaceProfileApiClient>();
        services.AddTransient<IUserApiClient, UserApiClient>();
        services.AddTransient<IUserIdentityApiClient, UserIdentityApiClient>();
    }
}
