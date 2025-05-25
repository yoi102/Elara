using ApiClients.Abstractions.ChatApiClient.Message;
using ApiClients.Abstractions.ChatApiClient.Message.Requests;
using ApiClients.Abstractions.Models.Responses;
using Services.Abstractions.ChatServices;

namespace Services.ChatServices;

internal class ChatMessageService : IChatMessageService
{
    private readonly IChatMessageApiClient chatMessageApiClient;

    public ChatMessageService(IChatMessageApiClient chatMessageApiClient)
    {
        this.chatMessageApiClient = chatMessageApiClient;
    }

    public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await chatMessageApiClient.DeleteByIdAsync(id, cancellationToken);

        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
    }

    public async Task<MessageWithReplyMessageData?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatMessageApiClient.FindByIdAsync(id, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            return response.ResponseData;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<MessageWithReplyMessageData[]> GetBatch(Guid[] ids, CancellationToken cancellationToken = default)
    {
        var response = await chatMessageApiClient.GetBatch(ids, cancellationToken);

        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

        return response.ResponseData;
    }

    public async Task<ReplyMessageData[]> GetReplyMessagesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await chatMessageApiClient.GetReplyMessagesAsync(id, cancellationToken);

        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

        return response.ResponseData;
    }

    public async Task ReplyMessageAsync(ReplyMessageRequest replyMessageRequest, CancellationToken cancellationToken = default)
    {
        var response = await chatMessageApiClient.ReplyMessageAsync(replyMessageRequest, cancellationToken);

        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
    }

    public async Task SendMessageAsync(MessageRequest messageData, CancellationToken cancellationToken = default)
    {
        var response = await chatMessageApiClient.SendMessageAsync(messageData, cancellationToken);

        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
    }

    public async Task UpdateMessageAsync(MessageRequest messageData, CancellationToken cancellationToken = default)
    {
        var response = await chatMessageApiClient.UpdateMessageAsync(messageData, cancellationToken);

        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
    }
}
