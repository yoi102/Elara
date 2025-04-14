using ASPNETCore;
using ChatService.Domain;
using ChatService.Domain.Entities;
using ChatService.Infrastructure;
using ChatService.WebAPI.Controllers.MessageController.Requests;
using ChatService.WebAPI.Controllers.MessageController.Responses;
using DomainCommons.EntityStronglyIds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ChatService.WebAPI.Controllers.MessageController;

[Authorize]
[ApiController]
[Route("api/message")]
public class MessageController : AuthorizedUserController
{
    private readonly ILogger<MessageController> logger;
    private readonly ChatServiceDbContext dbContext;
    private readonly ChatDomainService domainService;
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

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage(SendMessageRequest request)
    {
        var conversation = await repository.FindConversationByIdAsync(request.ConversationId);
        if (conversation is null)
            return NotFound();
        var senderId = GetCurrentUserId();
        var message = new Message(senderId, request.ConversationId, request.Content, request.QuoteMessage);
        await dbContext.Messages.AddAsync(message);

        var messageAttachments = request.UploadedItemIds.Select(m => new MessageAttachment(message.Id, m));
        await dbContext.MessageAttachments.AddRangeAsync(messageAttachments);

        var participants = await repository.GetConversationAllParticipantsAsync(request.ConversationId);
        var unreadMessages = participants.Where(x => x.UserId != senderId).Select(x => new UserUnreadMessage(x.UserId, message.Id));
        await dbContext.UserUnreadMessages.AddRangeAsync(unreadMessages);

        return Ok();
    }

    [HttpPost("reply")]
    public async Task<IActionResult> ReplyMessage(ReplyMessageRequest request)
    {
        var conversation = await repository.FindConversationByIdAsync(request.ConversationId);
        if (conversation is null)
            return NotFound();
        var message = await repository.FindMessageByIdAsync(request.MessageId);
        if (message is null)
            return NotFound();

        var senderId = GetCurrentUserId();
        var sendMessage = new Message(senderId, request.ConversationId, request.Content, request.QuoteMessage);
        var replyMessage = new ReplyMessage(request.MessageId, sendMessage.Id);
        await dbContext.Messages.AddAsync(sendMessage);
        await dbContext.ReplyMessages.AddAsync(replyMessage);

        var messageAttachments = request.MessageAttachment.Select(m => new MessageAttachment(sendMessage.Id, m));
        await dbContext.MessageAttachments.AddRangeAsync(messageAttachments);
        //todo：应该判断是否有@、、、、

        await dbContext.UserUnreadMessages.AddRangeAsync(new UserUnreadMessage(message.SenderId, message.Id));

        return Ok();
    }

    [HttpPatch("update")]
    public async Task<IActionResult> UpdateMessage(UpdateMessageRequest request)
    {
        var message = await repository.UpdateMessageAsync(request.MessageId, request.Content, request.MessageAttachment, request.QuoteMessage);
        if (message is null)
            return NotFound();

        return Ok(message);
    }

    [HttpGet("conversation-messages")]
    public async Task<IActionResult> AllConversationMessages([RequiredGuidStronglyId] ConversationId id)
    {
        var messages = await repository.GetConversationAllMessagesAsync(id);
        var messagesResponse = new List<MessageResponse>();
        foreach (var message in messages)
        {
            var messageAttachments = await repository.GetMessageAllMessageAttachmentsAsync(message.Id);
            var uploadedItemIds = messageAttachments.Select(x => x.UploadedItemId);

            var messageResponse = new MessageResponse()
            {
                Id = message.Id,
                ConversationId = message.ConversationId,
                QuoteMessages = message.QuoteMessageId,
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

    [HttpGet("message")]
    public async Task<IActionResult> FindMessage([RequiredGuidStronglyId] MessageId id)
    {
        var message = await repository.FindMessageByIdAsync(id);
        if (message is null)
            return NotFound();

        var messageAttachments = await repository.GetMessageAllMessageAttachmentsAsync(id);
        var uploadedItemIds = messageAttachments.Select(x => x.UploadedItemId);
        var messageResponse = new MessageResponse()
        {
            Id = message.Id,
            ConversationId = message.ConversationId,
            QuoteMessages = message.QuoteMessageId,
            Content = message.Content,
            SenderId = message.SenderId,
            UploadedItemIds = [.. uploadedItemIds],
            CreatedAt = message.CreatedAt,
            UpdatedAt = message.UpdatedAt
        };
        return Ok(messageResponse);
    }

    [HttpGet("reply-messages")]
    public async Task<IActionResult> AddReplyMessage([RequiredGuidStronglyId] MessageId id)
    {
        var message = await repository.FindMessageByIdAsync(id);
        if (message is null)
            return NotFound();

        var messagesResponse = new List<MessageResponse>();

        var replyMessages = await repository.MessageAllReplyMessagesAsync(id);

        foreach (var replyMessage in replyMessages)
        {
            var replyMessageAttachments = await repository.GetMessageAllMessageAttachmentsAsync(id);
            var uploadedItemIds = replyMessageAttachments.Select(x => x.UploadedItemId);

            var messageResponse = new MessageResponse()
            {
                Id = message.Id,
                ConversationId = message.ConversationId,
                QuoteMessages = message.QuoteMessageId,
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
}
