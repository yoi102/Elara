using ASPNETCore;
using ChatService.Domain;
using ChatService.Domain.Entities;
using ChatService.Infrastructure;
using ChatService.WebAPI.Controllers.ConversationController.Requests;
using ChatService.WebAPI.Controllers.Responses;
using DomainCommons.EntityStronglyIds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ChatService.WebAPI.Controllers.ConversationController;

[Authorize]
[ApiController]
[Route("api/conversation")]
public class ConversationController : AuthorizedUserController
{
    private readonly ChatServiceDbContext dbContext;
    private readonly ChatDomainService domainService;
    private readonly ILogger<ConversationController> logger;
    private readonly IChatServiceRepository repository;

    public ConversationController(ILogger<ConversationController> logger,
                                  ChatServiceDbContext dbContext,
                                  IChatServiceRepository repository,
                                  ChatDomainService domainService)
    {
        this.logger = logger;
        this.dbContext = dbContext;
        this.repository = repository;
        this.domainService = domainService;
    }

    [HttpGet("all-conversation")]
    public async Task<ActionResult<Conversation[]>> AllConversation()
    {
        return await repository.GetConversationsByUserIdAsync(GetCurrentUserId());
    }

    [HttpGet("{id}/messages")]
    public async Task<IActionResult> AllConversationMessages([RequiredGuidStronglyId] ConversationId id)
    {
        var messages = await repository.GetConversationAllMessagesAsync(id);
        if (messages is null || messages.Length == 0)
            return Ok(Array.Empty<MessageResponse>());

        var unreadMessages = await repository.GetUnreadMessagesAsync(GetCurrentUserId(), id);
        var unreadIds = unreadMessages.Select(x => x.MessageId).ToHashSet(); // 提前做成HashSet，加速判断

        var messagesResponse = new List<MessageResponse>();
        foreach (var message in messages)
        {
            var messageAttachments = await repository.GetMessageAllMessageAttachmentsAsync(message.Id);
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
            messagesResponse.Add(messageResponse);
        }

        return Ok(messagesResponse);
    }

    [HttpPatch()]
    public async Task<IActionResult> ChangeName(ChangeGroupConversationNameRequest request)
    {
        if (await dbContext.Conversations.AnyAsync(g => g.Name == request.NewName))
        {
            return Conflict();
        }

        var groupConversation = await domainService.ChangeConversationNameAsync(request.Id, request.NewName);

        if (groupConversation is null)
        {
            return NotFound();
        }

        return Ok(groupConversation);
    }

    [HttpPost("{targetUserId}/create-conversation")]
    public async Task<IActionResult> CreateConversation(UserId targetUserId)
    {
        var currentUserId = GetCurrentUserId();

        var targetUserParticipant = dbContext.Participants.Where(x => !x.IsGroup && x.UserId == targetUserId);
        var currentUserParticipant = dbContext.Participants.Where(x => !x.IsGroup && x.UserId == currentUserId);

        var intersection = await dbContext.Participants.Where(p => !p.IsGroup && (p.UserId == currentUserId || p.UserId == targetUserId))
                                                   .GroupBy(p => p.ConversationId)
                                                   .Where(g => g.Count() == 2) // 两个用户都参与这个对话
                                                   .Select(g => g.Key) // ConversationId
                                                   .SingleOrDefaultAsync();
        if (intersection != default)
        {
            var conversation = await dbContext.Conversations.FindAsync(intersection);
            if (conversation is not null)
                return Ok(conversation);
        }

        Conversation newConversation = new("", false);
        await dbContext.AddAsync(newConversation);

        await dbContext.Participants.AddAsync(new Participant(newConversation.Id, targetUserId));
        await dbContext.Participants.AddAsync(new Participant(newConversation.Id, currentUserId));

        return Created(nameof(FindById), newConversation);
    }

    [HttpPost("create-group-conversation")]
    public async Task<IActionResult> CreateGroupConversation(ConversationCreateRequest request)
    {
        if (await dbContext.Conversations.AnyAsync(g => g.Name == request.Name))
        {
            return Conflict();
        }

        Conversation newGroupConversation = new(request.Name, true);
        await dbContext.AddAsync(newGroupConversation);

        var conversationRequests = request.Member.Select(item => new ConversationRequest(GetCurrentUserId(), item.UserId, newGroupConversation.Id, item.Role));

        await dbContext.ConversationRequests.AddRangeAsync(conversationRequests);

        await dbContext.Participants.AddAsync(new Participant(newGroupConversation.Id, GetCurrentUserId(), Roles.Owner, true));

        return Created(nameof(FindById), newGroupConversation);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> FindById([RequiredGuidStronglyId] ConversationId id)
    {
        var conversation = await repository.FindConversationByIdAsync(id);

        if (conversation is null) NotFound();

        return Ok(conversation);
    }

    [HttpGet("{id}/latest-message")]
    public async Task<IActionResult> GetLatestMessage(ConversationId id)
    {
        var replyMessage = await repository.GetLatestMessage(id);
        if (replyMessage is null)
            return Ok(null);

        var unreadMessages = await repository.GetUnreadMessagesAsync(GetCurrentUserId(), id);
        var unreadIds = unreadMessages.Select(x => x.MessageId).ToHashSet();

        var messageAttachments = await repository.GetMessageAllMessageAttachmentsAsync(replyMessage.Id);
        var uploadedItemIds = messageAttachments.Select(x => x.UploadedItemId).ToArray();

        var messageResponse = new MessageResponse
        {
            IsUnread = unreadIds.Contains(replyMessage.Id),
            MessageId = replyMessage.Id,
            ConversationId = replyMessage.ConversationId,
            QuoteMessageId = replyMessage.QuoteMessageId,
            Content = replyMessage.Content,
            SenderId = replyMessage.SenderId,
            UploadedItemIds = uploadedItemIds,
            CreatedAt = replyMessage.CreatedAt,
            UpdatedAt = replyMessage.UpdatedAt
        };

        return Ok(messageResponse);
    }

    [HttpGet("{id}/latest-messages-before")]
    public async Task<IActionResult> GetMessagesBefore([FromRoute] ConversationId id, [FromQuery] DateTimeOffset before)
    {
        var messages = await repository.GetMessagesBefore(id, before);
        if (messages is null || messages.Length == 0)
            return Ok(Array.Empty<MessageResponse>());

        var unreadMessages = await repository.GetUnreadMessagesAsync(GetCurrentUserId(), id);
        var unreadIds = unreadMessages.Select(x => x.MessageId).ToHashSet();

        var messagesResponse = new List<MessageResponse>();

        foreach (var message in messages)
        {
            var messageAttachments = await repository.GetMessageAllMessageAttachmentsAsync(message.Id);
            var uploadedItemIds = messageAttachments.Select(x => x.UploadedItemId).ToArray();

            var messageResponse = new MessageResponse
            {
                IsUnread = unreadIds.Contains(message.Id),
                MessageId = message.Id,
                ConversationId = message.ConversationId,
                QuoteMessageId = message.QuoteMessageId,
                Content = message.Content,
                SenderId = message.SenderId,
                UploadedItemIds = uploadedItemIds,
                CreatedAt = message.CreatedAt,
                UpdatedAt = message.UpdatedAt
            };

            messagesResponse.Add(messageResponse);
        }

        return Ok(messagesResponse);
    }

    [HttpGet("{id}/unread-messages")]
    public async Task<IActionResult> GetUnreadMessages(ConversationId id)
    {
        var userUnreadMessages = await repository.GetUnreadMessagesAsync(GetCurrentUserId(), id);
        return Ok(userUnreadMessages);
    }

    [HttpDelete("{id}/mark-as-read")]
    public async Task<IActionResult> MarkMessagesAsRead(ConversationId id)
    {
        var userUnreadMessages = await repository.GetUnreadMessagesAsync(GetCurrentUserId(), id);
        dbContext.RemoveRange(userUnreadMessages);
        return Ok();
    }
}
