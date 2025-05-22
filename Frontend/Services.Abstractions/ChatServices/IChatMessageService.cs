using ApiClients.Abstractions.ChatApiClient.Message.Requests;
using ApiClients.Abstractions.Models.Responses;

namespace Services.Abstractions.ChatServices;

public interface IChatMessageService
{
    Task<ApiServiceResult> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<MessageWithReplyMessageData>> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<MessageWithReplyMessageData[]>> GetBatch(Guid[] ids, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<ReplyMessageData[]>> GetReplyMessagesAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiServiceResult> ReplyMessageAsync(ReplyMessageRequest replyMessageRequest, CancellationToken cancellationToken = default);

    Task<ApiServiceResult> SendMessageAsync(MessageRequest messageData, CancellationToken cancellationToken = default);

    Task<ApiServiceResult> UpdateMessageAsync(MessageRequest messageData, CancellationToken cancellationToken = default);
}
