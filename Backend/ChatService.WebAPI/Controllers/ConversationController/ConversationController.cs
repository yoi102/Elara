using ASPNETCore;
using ChatService.Domain;
using ChatService.Domain.Entities;
using ChatService.Infrastructure;
using ChatService.WebAPI.Controllers.ConversationController.Requests;
using DomainCommons.EntityStronglyIds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ChatService.WebAPI.Controllers.ConversationController;

[Authorize]
[ApiController]
[Route("api/group-conversations")]
public class ConversationController : ControllerBase
{
    private readonly ChatServiceDbContext dbContext;
    private readonly ChatDomainService domainService;
    private readonly IChatServiceRepository repository;

    public ConversationController(ChatServiceDbContext dbContext,
                                  IChatServiceRepository repository,
                                  ChatDomainService domainService)
    {
        this.dbContext = dbContext;
        this.repository = repository;
        this.domainService = domainService;
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<Conversation>> ChangeName(
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
    public async Task<ActionResult<Conversation>> Create(ConversationCreateRequest request)
    {
        if (await dbContext.Conversations.AnyAsync(g => g.Name == request.Name))
        {
            return Conflict();
        }

        Conversation newGroupConversation = new(request.Name, true);
        await dbContext.AddAsync(newGroupConversation);

        var members = request.Member.Select(item => new Participant(newGroupConversation.Id, item.UserId, item.Role));

        await dbContext.AddAsync(members);

        return Created();
    }

    [HttpGet("find-by-user-id")]
    public async Task<ActionResult<Conversation[]>> FindByUserId([FromQuery][Required][RequiredGuidStronglyId] UserId userId)
    {
        return await repository.GetConversationsByUserIdAsync(userId);
    }
}
