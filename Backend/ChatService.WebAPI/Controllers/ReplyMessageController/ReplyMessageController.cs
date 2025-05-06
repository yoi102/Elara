using ASPNETCore;
using ChatService.Domain;
using ChatService.Domain.Entities;
using ChatService.Infrastructure;
using ChatService.WebAPI.Controllers.MessageController.Requests;
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

        await dbContext.UserUnreadMessages.AddRangeAsync(new UserUnreadMessage(message.SenderId, message.Id));

        return Ok();
    }
}
