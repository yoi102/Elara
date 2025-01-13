using ChatService.Domain.Entities;
using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain;

public interface IChatServiceRepository
{
    #region GroupConversation

    Task<GroupConversation?> FindGroupConversationByIdAsync(GroupConversationId id);

    Task<GroupConversation[]> FindGroupConversationsByUserIdAsync(UserId id);

    #endregion GroupConversation

    #region GroupConversationMember
    Task<GroupConversationMember?> FindGroupConversationMemberByIdAsync(GroupConversationMemberId id);

    Task<GroupConversationMember[]> FindGroupConversationMemberByGroupConversationIdAsync(GroupConversationId id);
    #endregion


    #region GroupMessage

    Task<GroupMessage?> FindGroupMessageByIdAsync(MessageId id);

    Task<GroupMessage[]> FindGroupMessagesByGroupConversationIdAsync(GroupConversationId id);

    #endregion GroupMessage

    #region PersonalConversation

    Task<PersonalConversation?> FindPersonalConversationByIdAsync(PersonalConversationId id);

    #endregion PersonalConversation

    #region PersonalMessage

    Task<PersonalMessage?> FindPersonalMessageByIdAsync(MessageId id);

    Task<PersonalMessage[]> FindPersonalMessagesByPersonalConversationIdAsync(PersonalConversationId id);

    #endregion PersonalMessage

    #region ReplyMessage

    Task<ReplyMessage?> FindReplyMessageByIdAsync(MessageId id);

    Task<ReplyMessage[]> FindReplyMessagesByMessageIdAsync(MessageId id);

    #endregion ReplyMessage
}
