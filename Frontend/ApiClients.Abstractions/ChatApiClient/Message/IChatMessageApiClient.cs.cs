using ApiClients.Abstractions.ChatApiClient.Conversation.Responses;
using ApiClients.Abstractions.ChatApiClient.Message.Requests;
using ApiClients.Abstractions.ChatApiClient.Message.Responses;

namespace ApiClients.Abstractions.ChatApiClient.Message;

public interface IChatMessageApiClient
{
    Task<MessageResponse> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<MessageResponse> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<MessagesResponse> GetReplyMessagesAsync(Guid id, CancellationToken cancellationToken = default);

    Task<SendMessageResponse> SendMessageAsync(MessageRequest messageData, CancellationToken cancellationToken = default);

    Task<UpdateMessageResponse> UpdateMessageAsync(MessageRequest messageData, CancellationToken cancellationToken = default);
}
