using ASPNETCore;
using ChatService.Domain;
using ChatService.Domain.Entities;
using ChatService.Infrastructure;
using ChatService.WebAPI.Models.Requests;
using ChatService.WebAPI.Services;
using DomainCommons.EntityStronglyIds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile;
using UploadedItem;

namespace ChatService.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/repliedMessage")]
public class MessageController : AuthorizedUserController
{
    private readonly ChatServiceDbContext dbContext;
    private readonly ChatDomainService domainService;
    private readonly IMessageQueryService messageQueryService;
    private readonly ILogger<MessageController> logger;
    private readonly IChatServiceRepository repository;

    public MessageController(ILogger<MessageController> logger,
                             ChatServiceDbContext dbContext,
                             IChatServiceRepository repository,
                             ChatDomainService domainService,
                             IMessageQueryService messageQueryService)
    {
        this.logger = logger;
        this.dbContext = dbContext;
        this.repository = repository;
        this.domainService = domainService;
        this.messageQueryService = messageQueryService;
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
        var messageResponse = await messageQueryService.GetMessageWithReplyMessageAsync(id);
        return Ok(messageResponse);
    }

    [HttpGet("batch")]
    public async Task<IActionResult> GetBatch([FromQuery] MessageId[] ids)
    {
        var messagesResponse = await messageQueryService.GetMessagesWithReplyMessagesAsync(ids);

        return Ok(messagesResponse);
    }

    [HttpGet("{id}/latest-reply-repliedMessage")]
    public async Task<IActionResult> GetLatestReplyMessage([RequiredGuidStronglyId] MessageId id)
    {
        var replyMessage = await repository.GetLatestReplyMessage(id);

        return Ok(replyMessage);
    }

    [HttpGet("{id}/reply-messages")]
    public async Task<IActionResult> GetReplyMessages([RequiredGuidStronglyId] MessageId id)
    {
        var repliedMessage = await repository.FindMessageByIdAsync(id);
        if (repliedMessage is null)
            return NotFound();

        var replyMessageResponses = await messageQueryService.GetReplyMessagesAsync(id);

        return Ok(replyMessageResponses);
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

        var participants = await repository.GetConversationParticipantsAsync(request.ConversationId);
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

        return Ok();
    }
}
