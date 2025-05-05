using ApiClients.Abstractions.ChatApiClient.Message;

namespace ApiClients.Clients;
public class ChatMessageApiClient : IChatMessageApiClient
{
    private const string serviceUri = "/ChatService/api/message";
    private readonly ITokenRefreshingRestClient client;

    public ChatMessageApiClient(ITokenRefreshingRestClient client)
    {
        this.client = client;
    }
}
