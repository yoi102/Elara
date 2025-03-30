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

    public async Task<Conversation?> FindGroupConversationsByNameAsync(string name)
    {
        return await dbContext.Conversations
                    .FirstOrDefaultAsync(g => g.Name == name);
    }

    public async Task<Conversation[]> GetConversationsByUserIdAsync(UserId id)
    {
        var conversationIds =  dbContext.Participants
         .Where(p => p.UserId == id)
         .Select(p => p.ConversationId);

        return await dbContext.Conversations
            .Where(c => conversationIds.Contains(c.Id) && c.IsGroup)
            .ToArrayAsync();
    }

    #endregion Conversation

    #region Participant

    public async Task<Participant?> FindParticipantByIdAsync(ParticipantId id)
    {
        return await dbContext.FindAsync<Participant>(id);
    }

    public async Task<Participant[]> GetConversationAllParticipantsAsync(ConversationId id)
    {
        return await dbContext
                  .Participants
                  .Where(g => g.ConversationId == id)
                  .ToArrayAsync();
    }

    #endregion Participant

    #region Message

    public async Task<Message?> FindMessageByIdAsync(MessageId id)
    {
        return await dbContext.FindAsync<Message>(id);
    }

    public async Task<Message[]> GetConversationAllMessagesAsync(ConversationId id)
    {
        return await dbContext
                     .Messages
                     .Where(g => g.ConversationId == id)
                     .ToArrayAsync();
    }

    #endregion Message

    #region ReplyMessage

    public async Task<ReplyMessage?> FindReplyMessageByIdAsync(MessageId id)
    {
        return await dbContext.FindAsync<ReplyMessage>(id);
    }

    public async Task<ReplyMessage[]> MessageAllReplyMessagesAsync(MessageId id)
    {
        return await dbContext
                     .ReplyMessages
                     .Where(g => g.RepliedMessageId == id)
                     .ToArrayAsync();
    }

    #endregion ReplyMessage
}
