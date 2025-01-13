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
[Route("api/group-conversation")]
public class GroupConversationController : ControllerBase
{
    private readonly ChatServiceDbContext dbContext;
    private readonly IChatServiceRepository repository;

    public GroupConversationController(ChatServiceDbContext dbContext,
        IChatServiceRepository repository)
    {
        this.dbContext = dbContext;
        this.repository = repository;
    }

    [HttpPost]
    [Route("CreateGroupConversation")]
    public async Task<ActionResult<GroupConversation>> CreateGroupConversation(
        [Required] string name,
         [FromBody] GroupConversationAddMemberRequest[] request)
    {
        if (await dbContext.GroupConversations.AnyAsync(g => g.Name == name))
        {
            return Conflict();
        }

        GroupConversation newGroupConversation = new(name);
        await dbContext.AddAsync(newGroupConversation);

        await dbContext.SaveChangesAsync();

        var groupConversation = await repository.FindGroupConversationByIdAsync(newGroupConversation.Id);

        if (groupConversation is null)
        {
            return NotFound();
        }

        List<GroupConversationMember> members = [];
        foreach (var item in request)
        {
            members.Add(new GroupConversationMember(groupConversation.Id, item.UserId, item.Role));
        }
        await dbContext.AddAsync(members);

        return Created();
    }

    [Route("GroupConversationAddMember")]
    [HttpPost]
    public async Task<ActionResult> GroupConversationAddMember(
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

    [Route("ChangeGroupConversationName")]
    [HttpPatch]
    public async Task<ActionResult<GroupConversation>> ChangeGroupConversationName(
    [RequiredGuidStronglyId] GroupConversationId id,
    [Required][MinLength(1)] string name)
    {
        if (await dbContext.GroupConversations.AnyAsync(g => g.Name == name))
        {
            return Conflict();
        }

        var groupConversation = await repository.FindGroupConversationByIdAsync(id);

        if (groupConversation is null)
        {
            return NotFound();
        }
        groupConversation.ChangeName(name);

        return Ok(groupConversation);
    }









}
