using Services.Abstractions.Results;
using Services.Abstractions.Results.Data;

namespace Services.Abstractions;

public interface IConversationQueryService
{
    Task<ApiServiceResult<MessageData[]>> GetConversationMessagesAsync(Guid conversationId, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<ParticipantData[]>> GetConversationParticipantsAsync(Guid conversationId, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<ConversationData[]>> GetConversationsWithMessagesAsync(CancellationToken cancellationToken = default);

    Task<ApiServiceResult<MessageData[]>> GetConversationUnreadMessagesAsync(Guid conversationId, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<ReplyMessageData[]>> GetMessageReplyMessagesAsync(Guid messageId, CancellationToken cancellationToken = default);

    Task<ApiServiceSimpleResult<QuoteMessageData>> GetQuoteMessageAsync(Guid messageId, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<MessageSenderData>> GetSenderDataById(Guid senderId, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<UploadedItemData[]>> GetUploadedItemByIds(Guid[] fileIds, CancellationToken cancellationToken = default);
}
