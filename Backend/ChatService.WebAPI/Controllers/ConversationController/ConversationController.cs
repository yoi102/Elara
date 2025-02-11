using ASPNETCore;
using ChatService.Domain;
using ChatService.Domain.Entities;
using ChatService.Infrastructure;
using ChatService.WebAPI.Controllers.GroupConversationController.Requests;
using DomainCommons.EntityStronglyIds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ChatService.WebAPI.Controllers.GroupConversationController;

[Authorize]
[ApiController]
[Route("api/group-conversation")]
public class ConversationController : ControllerBase
{
    private readonly ChatServiceDbContext dbContext;
    private readonly DomainService domainService;
    private readonly IChatServiceRepository repository;

    public ConversationController(ChatServiceDbContext dbContext,
                                       IChatServiceRepository repository,
                                       DomainService domainService)
    {
        this.dbContext = dbContext;
        this.repository = repository;
        this.domainService = domainService;
    }

    [HttpPost("{id}")]
    public async Task<ActionResult> AddMember(
                                    [RequiredGuidStronglyId] ConversationId id,
                                    [FromBody] ConversationAddMemberRequest[] request)
    {
        var groupConversation = await repository.FindConversationByIdAsync(id);

        if (groupConversation is null)
        {
            return NotFound();
        }
        List<Participant> members = [];
        foreach (var item in request)
        {
            members.Add(new Participant(id, item.UserId, item.Role));
        }
        await dbContext.AddAsync(members);

        return Ok();
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<Conversation>> ChangeName(
                                             [RequiredGuidStronglyId] ConversationId id,
                                             [Required][MinLength(1)] string name)
    {
        if (await dbContext.GroupConversations.AnyAsync(g => g.Name == name))
        {
            return Conflict();
        }

        var groupConversation = await domainService.ChangeGroupConversationNameAsync(id, name);

        if (groupConversation is null)
        {
            return NotFound();
        }

        return Ok(groupConversation);
    }

    [HttpPost]
    public async Task<ActionResult<Conversation>> Create(ConversationCreateRequest request)
    {
        if (await dbContext.GroupConversations.AnyAsync(g => g.Name == request.Name))
        {
            return Conflict();
        }

        Conversation newGroupConversation = new(request.Name);
        await dbContext.AddAsync(newGroupConversation);

        List<Participant> members = [];
        foreach (var item in request.Member)
        {
            members.Add(new Participant(newGroupConversation.Id, item.UserId, item.Role));
        }
        await dbContext.AddAsync(members);

        return Created();
    }

    [HttpGet("find-by-name")]
    public async Task<ActionResult<Conversation>> FindByName([FromQuery][Required][MinLength(1)] string name)
    {
        var conversation = await repository.FindConversationsByNameAsync(name);
        if (conversation is null)
        {
            return NotFound();
        }

        return conversation;
    }

    [HttpGet("find-by-user-id")]
    public async Task<ActionResult<Conversation[]>> FindByUserId([FromQuery][Required][RequiredGuidStronglyId] UserId userId)
    {
        return await repository.FindConversationsByUserIdAsync(userId);
    }
}
