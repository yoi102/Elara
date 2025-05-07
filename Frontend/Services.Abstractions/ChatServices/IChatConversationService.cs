using ApiClients.Abstractions.ChatApiClient.Conversation.Requests;
using Services.Abstractions.Results;
using Services.Abstractions.Results.Results;

namespace Services.Abstractions.ChatServices;

public interface IChatConversationService
{
    Task<ApiServiceResult> ChangeNameAsync(Guid id, string newName, CancellationToken cancellationToken = default);

    Task<ConversationResult> CreateConversationAsync(Guid targetUserId, CancellationToken cancellationToken = default);

    Task<ConversationResult> CreateGroupConversationAsync(string name, IEnumerable<ConversationMemberRequest> memberRequests, CancellationToken cancellationToken = default);

    Task<ConversationResult> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ConversationsResult> GetAllConversationAsync(CancellationToken cancellationToken = default);

    Task<MessagesResult> GetAllConversationMessagesAsync(Guid id, CancellationToken cancellationToken = default);

    Task<UnreadMessagesResult> GetUnreadMessagesAsync(Guid id, CancellationToken cancellationToken = default);
}
