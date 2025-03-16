using ChatService.Domain.Entities;
using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain;

public interface IChatServiceRepository
{
    #region Conversation

    Task<Conversation?> FindConversationByIdAsync(ConversationId id);

    Task<Conversation?> FindGroupConversationsByNameAsync(string name);

    Task<Conversation[]> GetConversationsByUserIdAsync(UserId id);

    #endregion Conversation

    #region Participant

    Task<Participant?> FindParticipantByIdAsync(ParticipantId id);

    Task<Participant[]> GetConversationAllParticipantsAsync(ConversationId id);

    #endregion Participant

    #region Message

    Task<Message?> FindMessageByIdAsync(MessageId id);

    Task<Message[]> GetConversationAllMessagesAsync(ConversationId id);

    #endregion Message

    #region ReplyMessage

    Task<ReplyMessage?> FindReplyMessageByIdAsync(MessageId id);

    Task<ReplyMessage[]> MessageAllReplyMessagesAsync(MessageId id);

    #endregion ReplyMessage
}
