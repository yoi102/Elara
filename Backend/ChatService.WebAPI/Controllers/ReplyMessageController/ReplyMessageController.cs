using ASPNETCore;
using ChatService.Domain;
using ChatService.Domain.Entities;
using ChatService.Infrastructure;
using ChatService.WebAPI.Controllers.MessageController.Requests;
using ChatService.WebAPI.Controllers.Responses;
using DomainCommons.EntityStronglyIds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatService.WebAPI.Controllers.ReplyMessageController;

[Authorize]
[ApiController]
[Route("api/reply-message")]
public class ReplyMessageController : AuthorizedUserController
{
    private readonly ChatServiceDbContext dbContext;
    private readonly ChatDomainService domainService;
    private readonly ILogger<ReplyMessageController> logger;
    private readonly IChatServiceRepository repository;

    public ReplyMessageController(ILogger<ReplyMessageController> logger,
                                 ChatServiceDbContext dbContext,
                                 IChatServiceRepository repository,
                                 ChatDomainService domainService)
    {
        this.logger = logger;
        this.dbContext = dbContext;
        this.repository = repository;
        this.domainService = domainService;
    }

    [HttpPost()]
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

    [HttpGet("reply-messages")]
    public async Task<IActionResult> ReplyMessages([RequiredGuidStronglyId] MessageId id)
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
}
