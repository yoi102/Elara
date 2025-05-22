using ASPNETCore;
using ChatService.Domain;
using ChatService.Domain.Entities;
using ChatService.Infrastructure;
using ChatService.WebAPI.Models.Requests;
using ChatService.WebAPI.Models.Responses;
using ChatService.WebAPI.Services;
using DomainCommons.EntityStronglyIds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatService.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/conversation")]
public class ConversationController : AuthorizedUserController
{
    private readonly ChatServiceDbContext dbContext;
    private readonly ChatDomainService domainService;
    private readonly IMessageQueryService messageQueryService;
    private readonly ILogger<ConversationController> logger;
    private readonly IChatServiceRepository repository;

    public ConversationController(ILogger<ConversationController> logger,
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

    [HttpGet("all-conversation")]
    public async Task<IActionResult> GetAllConversationDetails()
    {
        var conversationEntities = await repository.GetConversationsByUserIdAsync(GetCurrentUserId());
        var conversationDetailsInfos = new List<ConversationDetailsResponse>();
        foreach (var conversation in conversationEntities)
        {
            var participants = await messageQueryService.GetConversationParticipants(conversation.Id);

            var messages = await messageQueryService.GetConversationAllMessagesWithReplyMessagesAsync(conversation.Id);

            var conversationDetailsInfo = new ConversationDetailsResponse()
            {
                Id = conversation.Id,
                Name = conversation.Name,
                IsGroup = conversation.IsGroup,
                CreatedAt = conversation.CreatedAt,
                Messages =messages,
                Participants = participants,
            };
            conversationDetailsInfos.Add(conversationDetailsInfo);
        }
        return Ok(conversationDetailsInfos);
    }

    [HttpGet("{id}/messages")]
    public async Task<IActionResult> AllConversationMessages([RequiredGuidStronglyId] ConversationId id)
    {
        var messages = await repository.GetConversationAllMessagesAsync(id);

        var messagesResponse = await messageQueryService.GetMessagesWithReplyMessagesAsync(messages);

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

        return Ok();
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

        var conversationInfo = new ConversationInfoResponse()
        {
            Id = newGroupConversation.Id,
            Name = newGroupConversation.Name,
            IsGroup = newGroupConversation.IsGroup,
            CreatedAt = newGroupConversation.CreatedAt,
        };

        return Created(nameof(FindById), conversationInfo);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> FindById([RequiredGuidStronglyId] ConversationId id)
    {
        var conversation = await repository.FindConversationByIdAsync(id);

        if (conversation is null)
            return NotFound();

        var conversationInfo = new ConversationInfoResponse()
        {
            Id = conversation.Id,
            Name = conversation.Name,
            IsGroup = conversation.IsGroup,
            CreatedAt = conversation.CreatedAt,
        };

        return Ok(conversationInfo);
    }

    [HttpGet("{id}/latest-message")]
    public async Task<IActionResult> GetLatestMessage(ConversationId id)
    {
        var conversation = await repository.FindConversationByIdAsync(id);
        if (conversation is null)
            return NotFound("Conversation not found"); // 该对话不存在

        var latestMessage = await repository.GetLatestMessage(id);
        if (latestMessage is null)
            return Ok(null);

        var message = await messageQueryService.GetMessageWithReplyMessageAsync(latestMessage.Id);

        return Ok(message);
    }

    [HttpGet("{id}/latest-messages-before")]
    public async Task<IActionResult> GetMessagesBefore([FromRoute] ConversationId id, [FromQuery] DateTimeOffset before)
    {
        var messages = await repository.GetMessagesBefore(id, before);

        var messagesResponse = await messageQueryService.GetMessagesWithReplyMessagesAsync(messages);

        return Ok(messagesResponse);
    }

    [HttpGet("{id}/unread-messages")]
    public async Task<IActionResult> GetUnreadMessages(ConversationId id)
    {
        var userUnreadMessages = await repository.GetUnreadMessagesAsync(GetCurrentUserId(), id);
        var userUnreadMessageIds = userUnreadMessages.Select(x => x.MessageId).ToArray();

        var messages = await repository.FindMessagesByIdsAsync(userUnreadMessageIds);

        var messagesResponse = await messageQueryService.GetMessagesWithReplyMessagesAsync(messages);

        return Ok(messagesResponse);
    }

    [HttpDelete("{id}/mark-as-read")]
    public async Task<IActionResult> MarkMessagesAsRead(ConversationId id)
    {
        var userUnreadMessages = await repository.GetUnreadMessagesAsync(GetCurrentUserId(), id);
        dbContext.RemoveRange(userUnreadMessages);
        return Ok();
    }

    [HttpGet("{id}/participants")]
    public async Task<IActionResult> GetConversationParticipants(ConversationId id)
    {
        var conversation = await repository.FindConversationByIdAsync(id);
        if (conversation is null)
            return NotFound("Conversation not found");

        var participants = await messageQueryService.GetConversationParticipants(id);

        return Ok(participants);
    }
}
