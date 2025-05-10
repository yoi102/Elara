using ApiClients.Abstractions.ChatApiClient.Conversation.Responses;
using ApiClients.Abstractions.ChatApiClient.Message.Requests;
using Services.Abstractions.Results;

namespace Services.Abstractions.ChatServices;

public interface IChatMessageService
{
    Task<ApiServiceResult> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<MessageData>> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<MessageData[]>> GetBatch(Guid[] ids, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<MessageData[]>> GetReplyMessagesAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiServiceResult> ReplyMessageAsync(ReplyMessageRequest replyMessageRequest, CancellationToken cancellationToken = default);

    Task<ApiServiceResult> SendMessageAsync(MessageRequest messageData, CancellationToken cancellationToken = default);

    Task<ApiServiceResult> UpdateMessageAsync(MessageRequest messageData, CancellationToken cancellationToken = default);
}
