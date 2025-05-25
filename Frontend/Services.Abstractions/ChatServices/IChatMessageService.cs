using ApiClients.Abstractions.ChatApiClient.Message.Requests;
using ApiClients.Abstractions.Models.Responses;

namespace Services.Abstractions.ChatServices;

public interface IChatMessageService
{
    Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<MessageWithReplyMessageData?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<MessageWithReplyMessageData[]> GetBatch(Guid[] ids, CancellationToken cancellationToken = default);

    Task<ReplyMessageData[]> GetReplyMessagesAsync(Guid id, CancellationToken cancellationToken = default);

    Task ReplyMessageAsync(ReplyMessageRequest replyMessageRequest, CancellationToken cancellationToken = default);

    Task SendMessageAsync(MessageRequest messageData, CancellationToken cancellationToken = default);

    Task UpdateMessageAsync(MessageRequest messageData, CancellationToken cancellationToken = default);
}
