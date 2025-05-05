using ApiClients.Abstractions.ChatApiClient.ConversationRequest;
using ApiClients.Abstractions.ChatApiClient.ConversationRequest.Responses;
using Frontend.Shared.Exceptions;
using RestSharp;

namespace ApiClients.Clients.ChatApiClient;

public class ChatConversationRequestApiClient : IChatConversationRequestApiClient
{
    private const string serviceUri = "/ChatService/api/conversation-request";
    private readonly ITokenRefreshingRestClient client;

    public ChatConversationRequestApiClient(ITokenRefreshingRestClient client)
    {
        this.client = client;
    }

    public async Task<AcceptConversationRequestResponse> AcceptConversationRequestAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{id}/accept",
            Method = Method.Patch
        };
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new AcceptConversationRequestResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        return new AcceptConversationRequestResponse() { IsSuccessful = true, StatusCode = response.StatusCode };
    }

    public async Task<ConversationRequestResponse> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{id}",
            Method = Method.Get
        };
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ConversationRequestResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        if (string.IsNullOrEmpty(response.Content))
            throw new ApiResponseException();

        var data = JsonUtils.DeserializeInsensitive<ConversationRequestData>(response.Content);

        if (data is null)
            throw new ApiResponseException();

        return new ConversationRequestResponse() { IsSuccessful = true, StatusCode = response.StatusCode, ResponseData = data };
    }

    public async Task<ConversationRequestsResponse> GetConversationRequestsAsync(CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri,
            Method = Method.Get
        };
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ConversationRequestsResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        if (string.IsNullOrEmpty(response.Content))
            throw new ApiResponseException();

        var data = JsonUtils.DeserializeInsensitive<ConversationRequestData[]>(response.Content);

        if (data is null)
            throw new ApiResponseException();

        return new ConversationRequestsResponse() { IsSuccessful = true, StatusCode = response.StatusCode, ResponseData = data };
    }

    public async Task<RejectConversationRequestResponse> RejectConversationRequestAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{id}/reject",
            Method = Method.Patch
        };
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new RejectConversationRequestResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        return new RejectConversationRequestResponse() { IsSuccessful = true, StatusCode = response.StatusCode };
    }

    public async Task<SendConversationRequestResponse> SendConversationRequestAsync(Guid receiverId, Guid conversationId, string role, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri,
            Method = Method.Post
        };
        request.AddBody(new
        {
            ReceiverId = receiverId,
            ConversationId = conversationId,
            Role = role,
        });
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new SendConversationRequestResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        if (string.IsNullOrEmpty(response.Content))
            throw new ApiResponseException();

        return new SendConversationRequestResponse() { IsSuccessful = true, StatusCode = response.StatusCode };
    }
}
