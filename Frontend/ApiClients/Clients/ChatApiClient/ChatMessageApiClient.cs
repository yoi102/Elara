using ApiClients.Abstractions.ChatApiClient.Conversation.Responses;
using ApiClients.Abstractions.ChatApiClient.Message;
using ApiClients.Abstractions.ChatApiClient.Message.Requests;
using ApiClients.Abstractions.ChatApiClient.Message.Responses;
using Frontend.Shared.Exceptions;
using RestSharp;

namespace ApiClients.Clients.ChatApiClient;

public class ChatMessageApiClient : IChatMessageApiClient
{
    private const string serviceUri = "/ChatService/api/message";
    private readonly ITokenRefreshingRestClient client;

    public ChatMessageApiClient(ITokenRefreshingRestClient client)
    {
        this.client = client;
    }

    public async Task<MessageResponse> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{id}",
            Method = Method.Delete
        };
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new MessageResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        return new MessageResponse() { IsSuccessful = true, StatusCode = response.StatusCode, ResponseData = data };
    }

    public async Task<MessageResponse> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{id}",
            Method = Method.Get
        };
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new MessageResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        if (string.IsNullOrEmpty(response.Content))
            throw new ApiResponseException();

        var data = JsonUtils.DeserializeInsensitive<MessageData>(response.Content);

        if (data is null)
            throw new ApiResponseException();

        return new MessageResponse() { IsSuccessful = true, StatusCode = response.StatusCode, ResponseData = data };
    }

    public async Task<MessagesResponse> GetReplyMessagesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"{id}/reply-messages",
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

    public async Task<SendMessageResponse> SendMessageAsync(MessageRequest messageData, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri,
            Method = Method.Post
        };
        request.AddBody(messageData);
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new SendMessageResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        return new SendMessageResponse() { IsSuccessful = true, StatusCode = response.StatusCode };
    }

    public async Task<UpdateMessageResponse> UpdateMessageAsync(MessageRequest messageData, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri,
            Method = Method.Patch
        };
        request.AddBody(messageData);
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new UpdateMessageResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        return new UpdateMessageResponse() { IsSuccessful = true, StatusCode = response.StatusCode };
    }
}
