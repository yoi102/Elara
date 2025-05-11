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

    Task<ApiServiceResult<MessageData[]>> GetAllConversationMessagesAsync(Guid id, DateTimeOffset before, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<MessageData[]>> GetConversationMessagesAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<ParticipantData[]>> GetConversationParticipantsAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiServiceSimpleResult<MessageData>> GetLatestMessage(Guid id, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<MessageData[]>> GetUnreadMessagesAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<ConversationData[]>> GetUserConversationsAsync(CancellationToken cancellationToken = default);
}
