using ApiClients.Abstractions.ChatApiClient.Conversation.Requests;
using ApiClients.Abstractions.Models;
using ApiClients.Abstractions.Models.Responses;

namespace ApiClients.Abstractions.ChatApiClient.Conversation;

public interface IChatConversationApiClient
{
    Task<ApiResponse> ChangeNameAsync(Guid id, string newName, CancellationToken cancellationToken = default);

    Task<ApiResponse<ConversationInfoData>> CreateConversationAsync(Guid targetUserId, CancellationToken cancellationToken = default);

    Task<ApiResponse<ConversationInfoData>> CreateGroupConversationAsync(string name, IEnumerable<ConversationMemberRequest> memberRequests, CancellationToken cancellationToken = default);

    Task<ApiResponse<ConversationInfoData>> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiResponse<MessageData[]>> GetConversationMessagesAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiResponse<ParticipantData[]>> GetConversationParticipantsAsync(Guid id, CancellationToken cancellationToken = default);

    Task<SimpleApiResponse<MessageData>> GetLatestMessage(Guid id, CancellationToken cancellationToken = default);

    Task<ApiResponse<MessageData[]>> GetMessagesBefore(Guid id, DateTimeOffset before, CancellationToken cancellationToken = default);

    Task<ApiResponse<MessageData[]>> GetUnreadMessagesAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiResponse<ConversationDetailsData[]>> GetUserConversationsAsync(CancellationToken cancellationToken = default);

    Task<ApiResponse> MarkMessagesAsReadAsync(Guid id, CancellationToken cancellationToken = default);
}
