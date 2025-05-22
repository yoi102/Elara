using ApiClients.Abstractions.ChatApiClient.Message.Requests;
using ApiClients.Abstractions.Models;
using ApiClients.Abstractions.Models.Responses;

namespace ApiClients.Abstractions.ChatApiClient.Message;

public interface IChatMessageApiClient
{
    Task<ApiResponse> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiResponse<MessageWithReplyMessageData>> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiResponse<MessageWithReplyMessageData[]>> GetBatch(Guid[] ids, CancellationToken cancellationToken = default);

    Task<ApiResponse<ReplyMessageData[]>> GetReplyMessagesAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiResponse> ReplyMessageAsync(ReplyMessageRequest replyMessageRequest, CancellationToken cancellationToken = default);

    Task<ApiResponse> SendMessageAsync(MessageRequest messageData, CancellationToken cancellationToken = default);

    Task<ApiResponse> UpdateMessageAsync(MessageRequest messageData, CancellationToken cancellationToken = default);
}
