﻿using ASPNETCore;
using ChatService.Domain;
using ChatService.Domain.Entities;
using ChatService.Infrastructure;
using ChatService.WebAPI.Controllers.MessageController.Requests;
using ChatService.WebAPI.Controllers.Responses;
using DomainCommons.EntityStronglyIds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatService.WebAPI.Controllers.MessageController;

[Authorize]
[ApiController]
[Route("api/message")]
public class MessageController : AuthorizedUserController
{
    private readonly ChatServiceDbContext dbContext;
    private readonly ChatDomainService domainService;
    private readonly ILogger<MessageController> logger;
    private readonly IChatServiceRepository repository;

    public MessageController(ILogger<MessageController> logger,
                             ChatServiceDbContext dbContext,
                             IChatServiceRepository repository,
                             ChatDomainService domainService)
    {
        this.logger = logger;
        this.dbContext = dbContext;
        this.repository = repository;
        this.domainService = domainService;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMessageById([RequiredGuidStronglyId] MessageId id)
    {
        var message = await repository.FindMessageByIdAsync(id);
        if (message is null)
            return Ok();

        dbContext.Messages.Remove(message);

        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> FindMessageById([RequiredGuidStronglyId] MessageId id)
    {
        var message = await repository.FindMessageByIdAsync(id);
        if (message is null)
            return NotFound();

        var unreadMessages = await repository.GetUserUnreadMessagesAsync(GetCurrentUserId());
        var unreadIds = unreadMessages.Select(x => x.MessageId).ToHashSet();

        var messageAttachments = await repository.GetMessageAllMessageAttachmentsAsync(id);
        var uploadedItemIds = messageAttachments.Select(x => x.UploadedItemId);
        var messageResponse = new MessageResponse()
        {
            IsUnread = unreadIds.Contains(message.Id),
            MessageId = message.Id,
            ConversationId = message.ConversationId,
            QuoteMessageId = message.QuoteMessageId,
            Content = message.Content,
            SenderId = message.SenderId,
            UploadedItemIds = [.. uploadedItemIds],
            CreatedAt = message.CreatedAt,
            UpdatedAt = message.UpdatedAt
        };
        return Ok(messageResponse);
    }

    [HttpGet("batch")]
    public async Task<IActionResult> GetBatch([FromQuery] MessageId[] ids)
    {
        var file = await repository.FindMessagesByIdsAsync(ids);
        return Ok(file);
    }

    [HttpGet("{id}/reply-messages")]
    public async Task<IActionResult> GetReplyMessagesId([RequiredGuidStronglyId] MessageId id)
    {
        var message = await repository.FindMessageByIdAsync(id);
        if (message is null)
            return NotFound();

        var messagesResponse = new List<MessageResponse>();

        var replyMessages = await repository.MessageAllReplyMessagesAsync(id);

        var unreadMessages = await repository.GetUserUnreadMessagesAsync(GetCurrentUserId());
        var unreadIds = unreadMessages.Select(x => x.MessageId).ToHashSet();
        foreach (var replyMessage in replyMessages)
        {
            var replyMessageAttachments = await repository.GetMessageAllMessageAttachmentsAsync(id);
            var uploadedItemIds = replyMessageAttachments.Select(x => x.UploadedItemId);

            var messageResponse = new MessageResponse()
            {
                IsUnread = unreadIds.Contains(message.Id),
                MessageId = message.Id,
                ConversationId = message.ConversationId,
                QuoteMessageId = message.QuoteMessageId,
                Content = message.Content,
                SenderId = message.SenderId,
                UploadedItemIds = [.. uploadedItemIds],
                CreatedAt = message.CreatedAt,
                UpdatedAt = message.UpdatedAt
            };
            messagesResponse.Add(messageResponse);
        }

        return Ok(messagesResponse);
    }

    [HttpPost()]
    public async Task<IActionResult> SendMessage(SendMessageRequest request)
    {
        var conversation = await repository.FindConversationByIdAsync(request.ConversationId);
        if (conversation is null)
            return NotFound();
        var senderId = GetCurrentUserId();
        var message = new Message(senderId, request.ConversationId, request.Content, request.QuoteMessage);
        await dbContext.Messages.AddAsync(message);

        var messageAttachments = request.MessageAttachmentIds.Select(m => new MessageAttachment(message.Id, m));
        await dbContext.MessageAttachments.AddRangeAsync(messageAttachments);

        var participants = await repository.GetConversationAllParticipantsAsync(request.ConversationId);
        var unreadMessages = participants.Where(x => x.UserId != senderId).Select(x => new UserUnreadMessage(x.UserId, message.Id, request.ConversationId));
        await dbContext.UserUnreadMessages.AddRangeAsync(unreadMessages);

        return Ok();
    }

    [HttpPatch()]
    public async Task<IActionResult> UpdateMessage(UpdateMessageRequest request)
    {
        var message = await repository.UpdateMessageAsync(request.MessageId, request.Content, request.MessageAttachmentIds, request.QuoteMessage);
        if (message is null)
            return NotFound();

        return Ok(message);
    }

    [HttpPost("reply-messages")]
    public async Task<IActionResult> ReplyMessage(ReplyMessageRequest request)
    {
        var conversation = await repository.FindConversationByIdAsync(request.ConversationId);
        if (conversation is null)
            return NotFound("Conversation not found");
        var message = await repository.FindMessageByIdAsync(request.MessageId);
        if (message is null)
            return NotFound("Message not found");

        var senderId = GetCurrentUserId();
        var sendMessage = new Message(senderId, request.ConversationId, request.Content, request.QuoteMessage);
        var replyMessage = new ReplyMessage(request.MessageId, sendMessage.Id);
        await dbContext.Messages.AddAsync(sendMessage);
        await dbContext.ReplyMessages.AddAsync(replyMessage);

        var messageAttachments = request.MessageAttachmentIds.Select(m => new MessageAttachment(sendMessage.Id, m));
        await dbContext.MessageAttachments.AddRangeAsync(messageAttachments);
        //todo：应该判断是否有@、、、、

        await dbContext.UserUnreadMessages.AddRangeAsync(new UserUnreadMessage(message.SenderId, message.Id, request.ConversationId));

        return Ok();
    }

    [HttpGet("{id}/latest-reply-message")]
    public async Task<IActionResult> GetLatestReplyMessage([RequiredGuidStronglyId] MessageId id)
    {
        var replyMessage = await repository.GetLatestReplyMessage(id);

        return Ok(replyMessage);
    }
}
