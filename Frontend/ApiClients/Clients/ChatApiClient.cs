using ApiClients.Abstractions.ChatApiClient;

namespace ApiClients.Clients;
public class ChatApiClient : IChatApiClient
{
    private readonly ITokenRefreshingRestClient tokenRefreshingRestClient;

    public ChatApiClient(ITokenRefreshingRestClient tokenRefreshingRestClient)
    {
        this.tokenRefreshingRestClient = tokenRefreshingRestClient;
    }






























}
