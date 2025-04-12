﻿using ChatService.Domain;
using ChatService.Domain.Entities;
using DomainCommons.EntityStronglyIds;
using DomainCommons.Enums;
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

    public async Task<Conversation[]> GetConversationsByUserIdAsync(UserId id)
    {
        var conversationIds = dbContext.Participants
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

    #region ConversationRequest

    public async Task<ConversationRequest[]> AllConversationRequestByReceiverIdAsync(UserId receiverId)
    {
        return await dbContext.ConversationRequests.Where(c => c.ReceiverId == receiverId).ToArrayAsync();
    }

    public async Task<ConversationRequest> CreateConversationRequestAsync(UserId senderId, UserId receiverId, ConversationId conversationId)
    {
        var request = await dbContext.ConversationRequests.SingleOrDefaultAsync(c => c.SenderId == senderId && c.ReceiverId == receiverId);
        if (request is not null)
            return request;

        var conversationRequest = new ConversationRequest(senderId, receiverId, conversationId);
        var entityEntry = await dbContext.ConversationRequests.AddAsync(conversationRequest);
        return entityEntry.Entity;
    }

    public async Task<ConversationRequest?> FindConversationRequestByIdAsync(ConversationRequestId conversationRequestId)
    {
        return await dbContext.ConversationRequests.FindAsync(conversationRequestId);
    }

    public async Task<ConversationRequest[]> GetPendingConversationRequestByReceiverIdAsync(UserId receiverId)
    {
        return await dbContext.ConversationRequests.Where(c => c.ReceiverId == receiverId && c.Status == RequestStatus.Pending).ToArrayAsync();
    }

    public async Task<ConversationRequest?> UpdateConversationRequestAsync(ConversationRequestId conversationRequestId, RequestStatus status)
    {
        var conversationRequest = await dbContext.ConversationRequests.FindAsync(conversationRequestId);
        if (conversationRequest == null)
            return null;
        conversationRequest.UpdateStatus(status);
        return conversationRequest;
    }

    #endregion ConversationRequest

    public async Task<Message?> UpdateMessageAsync(MessageId messageId, string content, UploadedItemId[] attachments)
    {
        var message = await FindMessageByIdAsync(messageId);

        if (message is null)
            return null;

        dbContext.MessageAttachments.RemoveRange(dbContext.MessageAttachments.Where(x => x.MessageId == messageId));

        var messageAttachments = new List<MessageAttachment>();
        foreach (var attachment in attachments)
        {
            var messageAttachment = new MessageAttachment(messageId, attachment);
            messageAttachments.Add(messageAttachment);
        }

        await dbContext.MessageAttachments.AddRangeAsync(messageAttachments);

        message?.ChangeContent(content);
        return message;
    }
}
