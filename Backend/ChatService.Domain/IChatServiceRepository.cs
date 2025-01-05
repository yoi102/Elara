using ChatService.Domain.Entities;
using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain
{
    public interface IChatServiceRepository
    {
        #region GroupConversation

        Task<GroupConversation?> FindGroupConversationByIdAsync(GroupConversationId id);

        Task<GroupConversation[]> FindGroupConversationsByUserIdAsync(UserId id);

        #endregion GroupConversation

        #region GroupMessage
        Task<GroupMessage?> FindGroupMessagesByIdAsync(MessageId id);

        Task<GroupMessage[]> FindGroupMessagesByGroupConversationIdAsync(GroupConversationId id);

        #endregion GroupMessage

        #region PersonalConversation

        Task<PersonalConversation?> FindPersonalConversationByIdAsync(PersonalConversationId id);

        #endregion PersonalConversation

        #region PersonalMessage

        Task<PersonalMessage?> FindPersonalMessagesByIdAsync(MessageId id);

        Task<PersonalMessage[]> FindPersonalMessagesByPersonalConversationIdAsync(PersonalConversationId id);

        #endregion PersonalMessage

        #region ReplyMessage

        Task<ReplyMessage?> FindReplyMessagesByIdAsync(MessageId id);

        Task<ReplyMessage[]> FindReplyMessagesByMessageIdAsync(MessageId id);

        #endregion ReplyMessage
    }
}
