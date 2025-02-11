using ChatService.Domain.Entities;
using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain;

public interface IChatServiceRepository
{
    #region Conversation

    Task<Conversation?> FindConversationByIdAsync(ConversationId id);

    Task<Conversation?> FindConversationsByNameAsync(string name);

    Task<Conversation[]> FindConversationsByUserIdAsync(UserId id);
    #endregion Conversation

    #region Participant

    Task<Participant?> FindParticipantByIdAsync(ParticipantId id);

    Task<Participant[]> FindParticipantsByConversationIdAsync(ConversationId id);

    #endregion Participant

    #region Message

    Task<Message?> FindMessageByIdAsync(MessageId id);

    Task<Message[]> FindMessagesByConversationIdAsync(ConversationId id);

    #endregion Message

    #region ReplyMessage

    Task<ReplyMessage?> FindReplyMessageByIdAsync(MessageId id);

    Task<ReplyMessage[]> FindReplyMessagesByMessageIdAsync(MessageId id);

    #endregion ReplyMessage
}
