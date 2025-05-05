using ApiClients.Abstractions.ChatApiClient.Participant;

namespace ApiClients.Clients;
public class ChatParticipantApiClient : IChatParticipantApiClient
{
    private const string serviceUri = "/ChatService/api/participant";
    private readonly ITokenRefreshingRestClient client;

    public ChatParticipantApiClient(ITokenRefreshingRestClient client)
    {
        this.client = client;
    }
}
