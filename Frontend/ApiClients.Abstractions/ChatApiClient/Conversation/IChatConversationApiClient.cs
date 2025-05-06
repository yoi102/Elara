using ApiClients.Abstractions.ChatApiClient.Conversation.Requests;
using ApiClients.Abstractions.ChatApiClient.Conversation.Responses;

namespace ApiClients.Abstractions.ChatApiClient.Conversation;

public interface IChatConversationApiClient
{
    Task<ApiResponse> ChangeNameAsync(Guid id, string newName, CancellationToken cancellationToken = default);

    Task<ConversationResponse> CreateConversationAsync(Guid targetUserId, CancellationToken cancellationToken = default);

    Task<ConversationResponse> CreateGroupConversationAsync(string name, IEnumerable<ConversationMemberRequest> memberRequests, CancellationToken cancellationToken = default);

    Task<ConversationResponse> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ConversationsResponse> GetAllConversationAsync(CancellationToken cancellationToken = default);

    Task<MessagesResponse> GetAllConversationMessagesAsync(Guid id, CancellationToken cancellationToken = default);
}
