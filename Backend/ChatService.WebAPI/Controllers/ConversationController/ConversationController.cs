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

    [HttpGet("messages")]
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

    [HttpPatch("{id}")]
    public async Task<IActionResult> ChangeName(
                                             [RequiredGuidStronglyId] ConversationId id,
                                             [Required][MinLength(1)] string name)
    {
        if (await dbContext.Conversations.AnyAsync(g => g.Name == name))
        {
            return Conflict();
        }

        var groupConversation = await domainService.ChangeConversationNameAsync(id, name);

        if (groupConversation is null)
        {
            return NotFound();
        }

        return Ok(groupConversation);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ConversationCreateRequest request)
    {
        if (await dbContext.Conversations.AnyAsync(g => g.Name == request.Name))
        {
            return Conflict();
        }

        Conversation newGroupConversation = new(request.Name, true);
        await dbContext.AddAsync(newGroupConversation);

        var members = request.Member.Select(item => new Participant(newGroupConversation.Id, item.UserId, item.Role));

        await dbContext.Participants.AddRangeAsync(members);
        await dbContext.Participants.AddAsync(new Participant(newGroupConversation.Id, GetCurrentUserId(), Roles.Owner));

        return Created();
    }

    [HttpGet("find-by-user-id")]
    public async Task<ActionResult<Conversation[]>> FindByUserId([FromQuery][Required][RequiredGuidStronglyId] UserId userId)
    {
        return await repository.GetConversationsByUserIdAsync(userId);
    }
}
