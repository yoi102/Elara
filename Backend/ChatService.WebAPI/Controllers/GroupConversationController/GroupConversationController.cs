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
public class GroupConversationController : ControllerBase
{
    private readonly ChatServiceDbContext dbContext;
    private readonly DomainService domainService;
    private readonly IChatServiceRepository repository;

    public GroupConversationController(ChatServiceDbContext dbContext,
                                       IChatServiceRepository repository,
                                       DomainService domainService)
    {
        this.dbContext = dbContext;
        this.repository = repository;
        this.domainService = domainService;
    }

    [HttpPost("{id}")]
    public async Task<ActionResult> AddMember(
                                    [RequiredGuidStronglyId] GroupConversationId id,
                                    [FromBody] GroupConversationAddMemberRequest[] request)
    {
        var groupConversation = await repository.FindGroupConversationByIdAsync(id);

        if (groupConversation is null)
        {
            return NotFound();
        }
        List<GroupConversationMember> members = [];
        foreach (var item in request)
        {
            members.Add(new GroupConversationMember(id, item.UserId, item.Role));
        }
        await dbContext.AddAsync(members);

        return Ok();
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<GroupConversation>> ChangeName(
                                             [RequiredGuidStronglyId] GroupConversationId id,
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
    public async Task<ActionResult<GroupConversation>> Create(GroupConversationCreateRequest request)
    {
        if (await dbContext.GroupConversations.AnyAsync(g => g.Name == request.Name))
        {
            return Conflict();
        }

        GroupConversation newGroupConversation = new(request.Name);
        await dbContext.AddAsync(newGroupConversation);

        List<GroupConversationMember> members = [];
        foreach (var item in request.Member)
        {
            members.Add(new GroupConversationMember(newGroupConversation.Id, item.UserId, item.Role));
        }
        await dbContext.AddAsync(members);

        return Created();
    }

    [HttpGet("name:{name}")]
    public async Task<ActionResult<GroupConversation>> FindByName([Required][MinLength(1)] string name)
    {
        var conversation = await repository.FindGroupConversationsByNameAsync(name);
        if (conversation is null)
        {
            return NotFound();
        }

        return conversation;
    }

    [HttpGet("userId:{userId}")]
    public async Task<ActionResult<GroupConversation[]>> FindByUserId([FromRoute][RequiredGuidStronglyId] UserId userId)
    {
        return await repository.FindGroupConversationsByUserIdAsync(userId);
    }
}
