using ApiClients.Abstractions.ChatApiClient.ReplyMessage;

namespace ApiClients.Clients;
public class ChatReplyMessageApiClient : IChatReplyMessageApiClient
{
    private const string serviceUri = "/ChatService/api/reply-message";
    private readonly ITokenRefreshingRestClient client;

    public ChatReplyMessageApiClient(ITokenRefreshingRestClient client)
    {
        this.client = client;
    }
}
