﻿using ApiClients.Abstractions.ChatApiClient.Message;
using ApiClients.Abstractions.ChatApiClient.Message.Requests;
using ApiClients.Abstractions.Models;
using ApiClients.Abstractions.Models.Responses;
using Frontend.Shared.Exceptions;
using RestSharp;

namespace ApiClients.Clients.ChatApiClient;

internal class ChatMessageApiClient : IChatMessageApiClient
{
    private const string serviceUri = "/ChatService/api/message";
    private readonly ITokenRefreshingRestClient client;

    public ChatMessageApiClient(ITokenRefreshingRestClient client)
    {
        this.client = client;
    }

    public async Task<ApiResponse> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{id}",
            Method = Method.Delete
        };
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ApiResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        return new ApiResponse() { IsSuccessful = true, StatusCode = response.StatusCode };
    }

    public async Task<ApiResponse<MessageWithReplyMessageData>> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{id}",
            Method = Method.Get
        };
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ApiResponse<MessageWithReplyMessageData> { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        if (string.IsNullOrEmpty(response.Content))
            throw new ApiResponseException();

        var data = JsonUtils.DeserializeInsensitive<MessageWithReplyMessageData>(response.Content);

        if (data is null)
            throw new ApiResponseException();

        return new ApiResponse<MessageWithReplyMessageData>() { IsSuccessful = true, StatusCode = response.StatusCode, ResponseData = data };
    }

    public async Task<ApiResponse<MessageWithReplyMessageData[]>> GetBatch(Guid[] ids, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + "/batch",
            Method = Method.Get
        };
        foreach (var id in ids)
        {
            request.AddQueryParameter("ids", id);
        }

        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ApiResponse<MessageWithReplyMessageData[]> { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        if (string.IsNullOrEmpty(response.Content))
            throw new ApiResponseException();

        var data = JsonUtils.DeserializeInsensitive<MessageWithReplyMessageData[]>(response.Content);

        if (data is null)
            throw new ApiResponseException();

        return new ApiResponse<MessageWithReplyMessageData[]>() { IsSuccessful = true, StatusCode = response.StatusCode, ResponseData = data };
    }

    public async Task<ApiResponse<ReplyMessageData[]>> GetReplyMessagesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"{id}/reply-messages",
            Method = Method.Get
        };
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ApiResponse<ReplyMessageData[]> { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        if (string.IsNullOrEmpty(response.Content))
            throw new ApiResponseException();

        var data = JsonUtils.DeserializeInsensitive<ReplyMessageData[]>(response.Content);

        if (data is null)
            throw new ApiResponseException();

        return new ApiResponse<ReplyMessageData[]>() { IsSuccessful = true, StatusCode = response.StatusCode, ResponseData = data };
    }

    public async Task<ApiResponse> ReplyMessageAsync(ReplyMessageRequest replyMessageRequest, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri,
            Method = Method.Post
        };
        request.AddBody(replyMessageRequest);
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ApiResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        return new ApiResponse() { IsSuccessful = true, StatusCode = response.StatusCode };
    }

    public async Task<ApiResponse> SendMessageAsync(MessageRequest messageData, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri,
            Method = Method.Post
        };
        request.AddBody(messageData);
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ApiResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        return new ApiResponse() { IsSuccessful = true, StatusCode = response.StatusCode };
    }

    public async Task<ApiResponse> UpdateMessageAsync(MessageRequest messageData, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri,
            Method = Method.Patch
        };
        request.AddBody(messageData);
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ApiResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        return new ApiResponse() { IsSuccessful = true, StatusCode = response.StatusCode };
    }
}
