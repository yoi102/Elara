using ApiClients.Abstractions.ChatApiClient.Conversation.Requests;
using ApiClients.Abstractions.Models.Responses;

namespace Services.Abstractions.ChatServices;

public interface IChatConversationService
{
    Task<ApiServiceResult> ChangeNameAsync(Guid id, string newName, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<ConversationInfoData>> CreateConversationAsync(Guid targetUserId, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<ConversationInfoData>> CreateGroupConversationAsync(string name, IEnumerable<ConversationMemberRequest> memberRequests, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<ConversationInfoData>> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<MessageData[]>> GetAllConversationMessagesAsync(Guid id, DateTimeOffset before, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<MessageData[]>> GetConversationMessagesAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<ParticipantData[]>> GetConversationParticipantsAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiServiceSimpleResult<MessageData>> GetLatestMessage(Guid id, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<MessageData[]>> GetUnreadMessagesAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<ConversationDetailsData[]>> GetUserConversationsAsync(CancellationToken cancellationToken = default);
}
