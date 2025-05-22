using ApiClients.Abstractions.ChatApiClient.Conversation;
using ApiClients.Abstractions.ChatApiClient.Conversation.Requests;
using ApiClients.Abstractions.Models;
using ApiClients.Abstractions.Models.Responses;
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

    public async Task<ApiResponse<ConversationInfoData>> CreateConversationAsync(Guid targetUserId, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{targetUserId}/create-conversation",
            Method = Method.Post
        };

        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ApiResponse<ConversationInfoData> { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        if (string.IsNullOrEmpty(response.Content))
            throw new ApiResponseException();

        var data = JsonUtils.DeserializeInsensitive<ConversationInfoData>(response.Content);

        if (data is null)
            throw new ApiResponseException();

        return new ApiResponse<ConversationInfoData>() { IsSuccessful = true, StatusCode = response.StatusCode, ResponseData = data };
    }

    public async Task<ApiResponse<ConversationInfoData>> CreateGroupConversationAsync(string name, IEnumerable<ConversationMemberRequest> memberRequests, CancellationToken cancellationToken = default)
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
            return new ApiResponse<ConversationInfoData> { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        if (string.IsNullOrEmpty(response.Content))
            throw new ApiResponseException();

        var data = JsonUtils.DeserializeInsensitive<ConversationInfoData>(response.Content);

        if (data is null)
            throw new ApiResponseException();

        return new ApiResponse<ConversationInfoData>() { IsSuccessful = true, StatusCode = response.StatusCode, ResponseData = data };
    }

    public async Task<ApiResponse<ConversationInfoData>> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{id}",
            Method = Method.Get
        };

        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ApiResponse<ConversationInfoData> { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        if (string.IsNullOrEmpty(response.Content))
            throw new ApiResponseException();

        var data = JsonUtils.DeserializeInsensitive<ConversationInfoData>(response.Content);

        if (data is null)
            throw new ApiResponseException();

        return new ApiResponse<ConversationInfoData>() { IsSuccessful = true, StatusCode = response.StatusCode, ResponseData = data };
    }

    public async Task<ApiResponse<MessageData[]>> GetConversationMessagesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{id}/messages",
            Method = Method.Get
        };
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ApiResponse<MessageData[]> { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        if (string.IsNullOrEmpty(response.Content))
            throw new ApiResponseException();

        var data = JsonUtils.DeserializeInsensitive<MessageData[]>(response.Content);

        if (data is null)
            throw new ApiResponseException();

        return new ApiResponse<MessageData[]>() { IsSuccessful = true, StatusCode = response.StatusCode, ResponseData = data };
    }

    public async Task<ApiResponse<ParticipantData[]>> GetConversationParticipantsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{id}/participants",
            Method = Method.Get
        };

        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ApiResponse<ParticipantData[]> { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        if (string.IsNullOrEmpty(response.Content))
            throw new ApiResponseException();

        var data = JsonUtils.DeserializeInsensitive<ParticipantData[]>(response.Content);

        if (data is null)
            throw new ApiResponseException();

        return new ApiResponse<ParticipantData[]>() { IsSuccessful = true, StatusCode = response.StatusCode, ResponseData = data };
    }

    public async Task<SimpleApiResponse<MessageData>> GetLatestMessage(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{id}/latest-message",
            Method = Method.Get
        };

        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new SimpleApiResponse<MessageData> { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        if (string.IsNullOrEmpty(response.Content))
            throw new ApiResponseException();

        var data = JsonUtils.DeserializeInsensitive<MessageData?>(response.Content);

        return new SimpleApiResponse<MessageData>() { IsSuccessful = true, StatusCode = response.StatusCode, ResponseData = data };
    }

    public async Task<ApiResponse<MessageData[]>> GetMessagesBefore(Guid id, DateTimeOffset before, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{id}/latest-messages-before",
            Method = Method.Get
        };
        request.AddQueryParameter("before", before);

        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ApiResponse<MessageData[]> { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        if (string.IsNullOrEmpty(response.Content))
            throw new ApiResponseException();

        var data = JsonUtils.DeserializeInsensitive<MessageData[]>(response.Content);

        if (data is null)
            throw new ApiResponseException();

        return new ApiResponse<MessageData[]>() { IsSuccessful = true, StatusCode = response.StatusCode, ResponseData = data };
    }

    public async Task<ApiResponse<MessageData[]>> GetUnreadMessagesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{id}/unread-messages",
            Method = Method.Get
        };
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ApiResponse<MessageData[]> { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        if (string.IsNullOrEmpty(response.Content))
            throw new ApiResponseException();

        var data = JsonUtils.DeserializeInsensitive<MessageData[]>(response.Content);

        if (data is null)
            throw new ApiResponseException();

        return new ApiResponse<MessageData[]>() { IsSuccessful = true, StatusCode = response.StatusCode, ResponseData = data };
    }

    public async Task<ApiResponse<ConversationDetailsData[]>> GetUserConversationsAsync(CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/all-conversation",
            Method = Method.Get
        };

        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ApiResponse<ConversationDetailsData[]> { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        if (string.IsNullOrEmpty(response.Content))
            throw new ApiResponseException();

        var data = JsonUtils.DeserializeInsensitive<ConversationDetailsData[]>(response.Content);

        if (data is null)
            throw new ApiResponseException();

        return new ApiResponse<ConversationDetailsData[]>() { IsSuccessful = true, StatusCode = response.StatusCode, ResponseData = data };
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
