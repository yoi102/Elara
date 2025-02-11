using ChatService.Domain;
using ChatService.Domain.Entities;
using DomainCommons.EntityStronglyIds;
using Microsoft.EntityFrameworkCore;

namespace ChatService.Infrastructure;

public class ChatServiceRepository : IChatServiceRepository
{
    private readonly ChatServiceDbContext dbContext;

    public ChatServiceRepository(ChatServiceDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    #region Conversation

    public async Task<Conversation?> FindConversationByIdAsync(ConversationId id)
    {
        return await dbContext.FindAsync<Conversation>(id);
    }

    public async Task<Conversation?> FindConversationsByNameAsync(string name)
    {
        return await dbContext.GroupConversations
                    .FirstOrDefaultAsync(g => g.Name == name);
    }

    public async Task<Conversation[]> FindConversationsByUserIdAsync(UserId id)
    {
        return await dbContext.GroupConversations
                    .Join(
                        dbContext.GroupConversationMembers.Where(gm => gm.UserId == id),
                        gc => gc.Id,
                        gm => gm.ConversationId,
                        (gc, gm) => gc
                    )
                    .ToArrayAsync();
    }

    #endregion Conversation

    #region ConversationMember

    public async Task<Participant[]> FindConversationParticipantByConversationIdAsync(ConversationId id)
    {
        return await dbContext
                  .GroupConversationMembers
                  .Where(g => g.ConversationId == id)
                  .ToArrayAsync();
    }

    public async Task<Participant?> FindConversationParticipantByIdAsync(ParticipantId id)
    {
        return await dbContext.FindAsync<Participant>(id);
    }

    #endregion ConversationMember

    #region ConversationMessage

    public async Task<ConversationMessage?> FindConversationMessageByIdAsync(MessageId id)
    {
        return await dbContext.FindAsync<ConversationMessage>(id);
    }

    public async Task<ConversationMessage[]> FindConversationMessagesByConversationIdAsync(ConversationId id)
    {
        return await dbContext
                     .GroupMessages
                     .Where(g => g.ConversationId == id)
                     .ToArrayAsync();
    }

    #endregion ConversationMessage

    #region ReplyMessage

    public async Task<ReplyMessage?> FindReplyMessageByIdAsync(MessageId id)
    {
        return await dbContext.FindAsync<ReplyMessage>(id);
    }

    public async Task<ReplyMessage[]> FindReplyMessagesByMessageIdAsync(MessageId id)
    {
        return await dbContext
                     .ReplyMessages
                     .Where(g => g.MessageId == id)
                     .ToArrayAsync();
    }

    #endregion ReplyMessage
}
