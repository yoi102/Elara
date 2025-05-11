using ApiClients.Abstractions.ChatApiClient.Conversation.Requests;
using ApiClients.Abstractions.ChatApiClient.Conversation.Responses;

namespace ApiClients.Abstractions.ChatApiClient.Conversation;

public interface IChatConversationApiClient
{
    Task<ApiResponse> ChangeNameAsync(Guid id, string newName, CancellationToken cancellationToken = default);

    Task<ConversationResponse> CreateConversationAsync(Guid targetUserId, CancellationToken cancellationToken = default);

    Task<ConversationResponse> CreateGroupConversationAsync(string name, IEnumerable<ConversationMemberRequest> memberRequests, CancellationToken cancellationToken = default);

    Task<ConversationResponse> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<MessagesResponse> GetConversationMessagesAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ParticipantsResponse> GetConversationParticipantsAsync(Guid id, CancellationToken cancellationToken = default);

    Task<SimpleMessageResponse> GetLatestMessage(Guid id, CancellationToken cancellationToken = default);

    Task<MessagesResponse> GetMessagesBefore(Guid id, DateTimeOffset before, CancellationToken cancellationToken = default);

    Task<MessagesResponse> GetUnreadMessagesAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ConversationsResponse> GetUserConversationsAsync(CancellationToken cancellationToken = default);

    Task<ApiResponse> MarkMessagesAsReadAsync(Guid id, CancellationToken cancellationToken = default);
}
