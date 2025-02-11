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

    #region Participant

    public async Task<Participant?> FindParticipantByIdAsync(ParticipantId id)
    {
        return await dbContext.FindAsync<Participant>(id);
    }

    public async Task<Participant[]> FindParticipantsByConversationIdAsync(ConversationId id)
    {
        return await dbContext
                  .GroupConversationMembers
                  .Where(g => g.ConversationId == id)
                  .ToArrayAsync();
    }

    #endregion Participant

    #region Message

    public async Task<Message?> FindMessageByIdAsync(MessageId id)
    {
        return await dbContext.FindAsync<Message>(id);
    }

    public async Task<Message[]> FindMessagesByConversationIdAsync(ConversationId id)
    {
        return await dbContext
                     .GroupMessages
                     .Where(g => g.ConversationId == id)
                     .ToArrayAsync();
    }

    #endregion Message

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
