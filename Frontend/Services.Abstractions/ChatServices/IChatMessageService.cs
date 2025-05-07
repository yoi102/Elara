using ApiClients.Abstractions.ChatApiClient.Message.Requests;
using Services.Abstractions.Results;
using Services.Abstractions.Results.Results;

namespace Services.Abstractions.ChatServices;

public interface IChatMessageService
{
    Task<ApiServiceResult> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<MessageResult> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<MessagesResult> GetReplyMessagesAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiServiceResult> ReplyMessageAsync(ReplyMessageRequest replyMessageRequest, CancellationToken cancellationToken = default);

    Task<ApiServiceResult> SendMessageAsync(MessageRequest messageData, CancellationToken cancellationToken = default);

    Task<ApiServiceResult> UpdateMessageAsync(MessageRequest messageData, CancellationToken cancellationToken = default);
}
