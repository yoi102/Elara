using ApiClients.Abstractions.ChatApiClient.Conversation;
using ApiClients.Abstractions.ChatApiClient.Conversation.Requests;
using ApiClients.Abstractions.ChatApiClient.Conversation.Responses;
using Frontend.Shared.Exceptions;
using RestSharp;

namespace ApiClients.Clients;

public class ChatConversationApiClient : IChatConversationApiClient
{
    private const string serviceUri = "/ChatService/api/conversation";
    private readonly ITokenRefreshingRestClient client;

    public ChatConversationApiClient(ITokenRefreshingRestClient client)
    {
        this.client = client;
    }

    public async Task<ChangeConversationNameResponse> ChangeNameAsync(Guid conversationId, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{conversationId}",
            Method = Method.Patch
        };
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ChangeConversationNameResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        return new ChangeConversationNameResponse() { IsSuccessful = true, StatusCode = response.StatusCode };
    }

    public async Task<ConversationResponse> CreateConversationAsync(Guid targetUserId, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{targetUserId}/create-conversation",
            Method = Method.Post
        };

        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ConversationResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        if (string.IsNullOrEmpty(response.Content))
            throw new ApiResponseException();

        var data = JsonUtils.DeserializeInsensitive<ConversationData>(response.Content);

        if (data is null)
            throw new ApiResponseException();

        return new ConversationResponse() { IsSuccessful = true, StatusCode = response.StatusCode, ResponseData = data };
    }

    public async Task<ConversationResponse> CreateGroupConversationAsync(string name, IEnumerable<ConversationMemberRequest> memberRequests, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + "create-group-conversation",
            Method = Method.Post
        };

        request.AddBody(new
        {
            Name = name,
            Member = memberRequests
        });
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ConversationResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        if (string.IsNullOrEmpty(response.Content))
            throw new ApiResponseException();

        var data = JsonUtils.DeserializeInsensitive<ConversationData>(response.Content);

        if (data is null)
            throw new ApiResponseException();

        return new ConversationResponse() { IsSuccessful = true, StatusCode = response.StatusCode, ResponseData = data };
    }

    public async Task<ConversationResponse> FindByIdAsync(Guid conversationUserId, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{conversationUserId}",
            Method = Method.Get
        };

        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ConversationResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        if (string.IsNullOrEmpty(response.Content))
            throw new ApiResponseException();

        var data = JsonUtils.DeserializeInsensitive<ConversationData>(response.Content);

        if (data is null)
            throw new ApiResponseException();

        return new ConversationResponse() { IsSuccessful = true, StatusCode = response.StatusCode, ResponseData = data };
    }

    public async Task<ConversationsResponse> GetAllConversationAsync(CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/all-conversation",
            Method = Method.Get
        };

        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ConversationsResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        if (string.IsNullOrEmpty(response.Content))
            throw new ApiResponseException();

        var data = JsonUtils.DeserializeInsensitive<ConversationData[]>(response.Content);

        if (data is null)
            throw new ApiResponseException();

        return new ConversationsResponse() { IsSuccessful = true, StatusCode = response.StatusCode, ResponseData = data };
    }

    public async Task<GetAllConversationMessagesResponse> GetAllConversationMessagesAsync(Guid conversationId, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{conversationId}/messages",
            Method = Method.Get
        };
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new GetAllConversationMessagesResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        if (string.IsNullOrEmpty(response.Content))
            throw new ApiResponseException();

        var data = JsonUtils.DeserializeInsensitive<ConversationMessageData[]>(response.Content);

        if (data is null)
            throw new ApiResponseException();

        return new GetAllConversationMessagesResponse() { IsSuccessful = true, StatusCode = response.StatusCode, ResponseData = data };
    }
}
