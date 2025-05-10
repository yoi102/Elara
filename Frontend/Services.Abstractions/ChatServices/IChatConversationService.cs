using ApiClients.Abstractions.ChatApiClient.Conversation.Requests;
using ApiClients.Abstractions.ChatApiClient.Conversation.Responses;
using Services.Abstractions.Results;

namespace Services.Abstractions.ChatServices;

public interface IChatConversationService
{
    Task<ApiServiceResult> ChangeNameAsync(Guid id, string newName, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<ConversationData>> CreateConversationAsync(Guid targetUserId, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<ConversationData>> CreateGroupConversationAsync(string name, IEnumerable<ConversationMemberRequest> memberRequests, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<ConversationData>> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<ConversationData[]>> GetAllConversationAsync(CancellationToken cancellationToken = default);

    Task<ApiServiceResult<MessageData[]>> GetAllConversationMessagesAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<UnreadMessageData[]>> GetUnreadMessagesAsync(Guid id, CancellationToken cancellationToken = default);
}
