using ASPNETCore;
using ChatService.Domain;
using ChatService.Domain.Entities;
using ChatService.Infrastructure;
using DomainCommons.EntityStronglyIds;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChatService.WebAPI.Controllers.PersonalConversationController;

[Route("api/personal-conversation")]
[ApiController]
public class PersonalConversationController : ControllerBase
{
    private readonly ChatServiceDbContext dbContext;
    private readonly IChatServiceRepository repository;
    private readonly DomainService domainService;

    public PersonalConversationController(ChatServiceDbContext dbContext,
                                       IChatServiceRepository repository,
                                       DomainService domainService)
    {
        this.dbContext = dbContext;
        this.repository = repository;
        this.domainService = domainService;
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<PersonalConversation>> Get([RequiredGuidStronglyId] PersonalConversationId id)
    {
        var personalConversation = await repository.FindPersonalConversationByIdAsync(id);

        if (personalConversation is null)
        {
            return NotFound();
        }

        return personalConversation;
    }

    [HttpGet()]
    public async Task<ActionResult<PersonalConversation>> FindByUserId([Required][RequiredGuidStronglyId] UserId userId)
    {
        var personalConversation = await repository.FindPersonalConversationByUserIdAsync(userId);

        if (personalConversation is null)
        {
            return NotFound();
        }

        return personalConversation;
    }


    [HttpPost]
    public async Task<ActionResult> Create([RequiredGuidStronglyId] UserId user1Id, [RequiredGuidStronglyId] UserId user2Id)
    {
        var personalConversation = new PersonalConversation(user1Id, user2Id);
        await dbContext.AddAsync(personalConversation);
        return Created();
    }

   

}
