using ChatService.Domain;
using ChatService.Domain.Entities;
using DomainCommons.EntityStronglyIds;
using Microsoft.EntityFrameworkCore;

namespace ChatService.Infrastructure
{
    public class ChatServiceRepository : IChatServiceRepository
    {
        private readonly ChatServiceDbContext dbContext;

        public ChatServiceRepository(ChatServiceDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        #region GroupConversation

        public async Task<GroupConversation?> FindGroupConversationByIdAsync(GroupConversationId id)
        {
            return await dbContext.FindAsync<GroupConversation>(id);
        }

        public async Task<GroupConversation[]> FindGroupConversationsByUserIdAsync(UserId id)
        {
            return await dbContext
                         .GroupConversations
                         .Where(g => g.Member.Contains(id))
                         .ToArrayAsync();
        }

        #endregion GroupConversation

        #region GroupMessage

        public async Task<GroupMessage[]> FindGroupMessagesByGroupConversationIdAsync(GroupConversationId id)
        {
            return await dbContext
                         .GroupMessages
                         .Where(g => g.GroupConversationId == id)
                         .ToArrayAsync();
        }

        public async Task<GroupMessage?> FindGroupMessagesByIdAsync(MessageId id)
        {
            return await dbContext.FindAsync<GroupMessage>(id);
        }

        #endregion GroupMessage

        #region PersonalConversation

        public async Task<PersonalConversation?> FindPersonalConversationByIdAsync(PersonalConversationId id)
        {
            return await dbContext.FindAsync<PersonalConversation>(id);
        }

        public async Task<PersonalMessage?> FindPersonalMessagesByIdAsync(MessageId id)
        {
            return await dbContext.FindAsync<PersonalMessage>(id);
        }

        #endregion PersonalConversation

        #region PersonalMessage

        public async Task<PersonalMessage[]> FindPersonalMessagesByPersonalConversationIdAsync(PersonalConversationId id)
        {
            return await dbContext
                         .PersonalMessages
                         .Where(g => g.PersonalConversationId == id)
                         .ToArrayAsync();
        }

        public async Task<ReplyMessage?> FindReplyMessagesByIdAsync(MessageId id)
        {
            return await dbContext.FindAsync<ReplyMessage>(id);
        }

        #endregion PersonalMessage

        #region ReplyMessage

        public async Task<ReplyMessage[]> FindReplyMessagesByMessageIdAsync(MessageId id)
        {
            return await dbContext
                         .ReplyMessages
                         .Where(g => g.MessageId == id)
                         .ToArrayAsync();
        }

        #endregion ReplyMessage
    }
}
