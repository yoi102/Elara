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
        var messagesResponse = new List<MessageResponse>();
        foreach (var message in messages)
        {
            var messageAttachments = await repository.GetMessageAllMessageAttachmentsAsync(message.Id);
            var uploadedItemIds = messageAttachments.Select(x => x.UploadedItemId);

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
        Conversation newConversation = new("", false);
        await dbContext.AddAsync(newConversation);

        await dbContext.Participants.AddAsync(new Participant(newConversation.Id, targetUserId, Roles.Owner));
        await dbContext.Participants.AddAsync(new Participant(newConversation.Id, GetCurrentUserId(), Roles.Owner));

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

        await dbContext.Participants.AddAsync(new Participant(newGroupConversation.Id, GetCurrentUserId(), Roles.Owner));

        return Created(nameof(FindById), newGroupConversation);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> FindById([RequiredGuidStronglyId] ConversationId id)
    {
        var conversation = await repository.FindConversationByIdAsync(id);

        if (conversation is null) NotFound();

        return Ok(conversation);
    }
}
