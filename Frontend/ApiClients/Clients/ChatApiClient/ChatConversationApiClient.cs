using ApiClients.Abstractions;
using ApiClients.Abstractions.ChatApiClient.Conversation;
using ApiClients.Abstractions.ChatApiClient.Conversation.Requests;
using ApiClients.Abstractions.ChatApiClient.Conversation.Responses;
using Frontend.Shared.Exceptions;
using RestSharp;

namespace ApiClients.Clients.ChatApiClient;

internal class ChatConversationApiClient : IChatConversationApiClient
{
    private const string serviceUri = "/ChatService/api/conversation";
    private readonly ITokenRefreshingRestClient client;

    public ChatConversationApiClient(ITokenRefreshingRestClient client)
    {
        this.client = client;
    }

    public async Task<ApiResponse> ChangeNameAsync(Guid id, string newName, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri,
            Method = Method.Patch
        };
        request.AddBody(new
        {
            Id = id,
            NewName = newName
        });

        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ApiResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        return new ApiResponse() { IsSuccessful = true, StatusCode = response.StatusCode };
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

    public async Task<ConversationResponse> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{id}",
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

    public async Task<MessagesResponse> GetConversationMessagesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{id}/messages",
            Method = Method.Get
        };
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new MessagesResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        if (string.IsNullOrEmpty(response.Content))
            throw new ApiResponseException();

        var data = JsonUtils.DeserializeInsensitive<MessageData[]>(response.Content);

        if (data is null)
            throw new ApiResponseException();

        return new MessagesResponse() { IsSuccessful = true, StatusCode = response.StatusCode, ResponseData = data };
    }

    public async Task<ParticipantsResponse> GetConversationParticipantsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{id}/participants",
            Method = Method.Get
        };

        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ParticipantsResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        if (string.IsNullOrEmpty(response.Content))
            throw new ApiResponseException();

        var data = JsonUtils.DeserializeInsensitive<ParticipantData[]>(response.Content);

        if (data is null)
            throw new ApiResponseException();

        return new ParticipantsResponse() { IsSuccessful = true, StatusCode = response.StatusCode, ResponseData = data };
    }

    public async Task<SimpleMessageResponse> GetLatestMessage(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{id}/latest-message",
            Method = Method.Get
        };

        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new SimpleMessageResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        if (string.IsNullOrEmpty(response.Content))
            throw new ApiResponseException();

        var data = JsonUtils.DeserializeInsensitive<MessageData?>(response.Content);

        return new SimpleMessageResponse() { IsSuccessful = true, StatusCode = response.StatusCode, ResponseData = data };
    }

    public async Task<MessagesResponse> GetMessagesBefore(Guid id, DateTimeOffset before, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{id}/latest-messages-before",
            Method = Method.Get
        };
        request.AddQueryParameter("before", before);

        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new MessagesResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        if (string.IsNullOrEmpty(response.Content))
            throw new ApiResponseException();

        var data = JsonUtils.DeserializeInsensitive<MessageData[]>(response.Content);

        if (data is null)
            throw new ApiResponseException();

        return new MessagesResponse() { IsSuccessful = true, StatusCode = response.StatusCode, ResponseData = data };
    }

    public async Task<MessagesResponse> GetUnreadMessagesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{id}/unread-messages",
            Method = Method.Get
        };
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new MessagesResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        if (string.IsNullOrEmpty(response.Content))
            throw new ApiResponseException();

        var data = JsonUtils.DeserializeInsensitive<MessageData[]>(response.Content);

        if (data is null)
            throw new ApiResponseException();

        return new MessagesResponse() { IsSuccessful = true, StatusCode = response.StatusCode, ResponseData = data };
    }

    public async Task<ConversationsResponse> GetUserConversationsAsync(CancellationToken cancellationToken = default)
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

    public async Task<ApiResponse> MarkMessagesAsReadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{id}/mark-as-read",
            Method = Method.Delete
        };
        request.AddBody(new
        {
            Id = id,
        });

        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ApiResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        return new ApiResponse() { IsSuccessful = true, StatusCode = response.StatusCode };
    }
}
