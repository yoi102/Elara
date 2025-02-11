using ChatService.Domain.Entities;
using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain;

public interface IChatServiceRepository
{
    #region Conversation

    Task<Conversation?> FindConversationByIdAsync(ConversationId id);

    Task<Conversation[]> FindConversationsByUserIdAsync(UserId id);

    Task<Conversation?> FindConversationsByNameAsync(string name);

    #endregion Conversation

    #region ConversationParticipant
    Task<Participant?> FindConversationParticipantByIdAsync(ParticipantId id);

    Task<Participant[]> FindConversationParticipantByConversationIdAsync(ConversationId id);
    #endregion ConversationParticipant

    #region ConversationMessage

    Task<ConversationMessage?> FindConversationMessageByIdAsync(MessageId id);

    Task<ConversationMessage[]> FindConversationMessagesByConversationIdAsync(ConversationId id);

    #endregion ConversationMessage

    #region ReplyMessage

    Task<ReplyMessage?> FindReplyMessageByIdAsync(MessageId id);

    Task<ReplyMessage[]> FindReplyMessagesByMessageIdAsync(MessageId id);

    #endregion ReplyMessage
}
