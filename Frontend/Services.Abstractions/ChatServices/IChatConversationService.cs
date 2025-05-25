using ApiClients.Abstractions.ChatApiClient.Conversation.Requests;
using ApiClients.Abstractions.Models.Responses;

namespace Services.Abstractions.ChatServices;

public interface IChatConversationService
{
    Task ChangeNameAsync(Guid id, string newName, CancellationToken cancellationToken = default);

    Task<ConversationInfoData> CreateConversationAsync(Guid targetUserId, CancellationToken cancellationToken = default);

    Task<ConversationInfoData> CreateGroupConversationAsync(string name, IEnumerable<ConversationMemberRequest> memberRequests, CancellationToken cancellationToken = default);

    Task<ConversationInfoData?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<MessageData[]> GetAllConversationMessagesAsync(Guid id, DateTimeOffset before, CancellationToken cancellationToken = default);

    Task<MessageData[]> GetConversationMessagesAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ParticipantData[]> GetConversationParticipantsAsync(Guid id, CancellationToken cancellationToken = default);

    Task<MessageData?> GetLatestMessage(Guid id, CancellationToken cancellationToken = default);

    Task<MessageData[]> GetUnreadMessagesAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ConversationDetailsData[]> GetUserConversationsAsync(CancellationToken cancellationToken = default);
}
