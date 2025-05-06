using ApiClients.Abstractions.ChatApiClient.ReplyMessage;
using ApiClients.Abstractions.ChatApiClient.ReplyMessage.Requests;
using ApiClients.Abstractions.ChatApiClient.ReplyMessage.Responses;
using RestSharp;

namespace ApiClients.Clients.ChatApiClient;

internal class ChatReplyMessageApiClient : IChatReplyMessageApiClient
{
    private const string serviceUri = "/ChatService/api/reply-message";
    private readonly ITokenRefreshingRestClient client;

    public ChatReplyMessageApiClient(ITokenRefreshingRestClient client)
    {
        this.client = client;
    }

    public async Task<ReplyMessageResponse> ReplyMessageAsync(ReplyMessageRequest replyMessageRequest, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri,
            Method = Method.Post
        };
        request.AddBody(replyMessageRequest);
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ReplyMessageResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        return new ReplyMessageResponse() { IsSuccessful = true, StatusCode = response.StatusCode };
    }
}
