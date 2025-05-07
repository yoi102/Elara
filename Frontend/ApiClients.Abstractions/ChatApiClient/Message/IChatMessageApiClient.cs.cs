using ApiClients.Abstractions.ChatApiClient.Conversation.Responses;
using ApiClients.Abstractions.ChatApiClient.Message.Requests;
using ApiClients.Abstractions.ChatApiClient.Message.Responses;

namespace ApiClients.Abstractions.ChatApiClient.Message;

public interface IChatMessageApiClient
{
    Task<ApiResponse> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<MessageResponse> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<MessagesResponse> GetReplyMessagesAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiResponse> ReplyMessageAsync(ReplyMessageRequest replyMessageRequest, CancellationToken cancellationToken = default);

    Task<ApiResponse> SendMessageAsync(MessageRequest messageData, CancellationToken cancellationToken = default);

    Task<ApiResponse> UpdateMessageAsync(MessageRequest messageData, CancellationToken cancellationToken = default);
}
