using ASPNETCore;
using ChatService.Domain;
using ChatService.Infrastructure;
using ChatService.WebAPI.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatService.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/participant")]
public class ParticipantController : AuthorizedUserController
{
    private readonly ILogger<ParticipantController> logger;
    private readonly ChatServiceDbContext dbContext;
    private readonly ChatDomainService domainService;
    private readonly IChatServiceRepository repository;

    public ParticipantController(ILogger<ParticipantController> logger,
                                 ChatServiceDbContext dbContext,
                                 IChatServiceRepository repository,
                                 ChatDomainService domainService)
    {
        this.logger = logger;
        this.dbContext = dbContext;
        this.repository = repository;
        this.domainService = domainService;
    }

    [HttpPatch()]
    public async Task<IActionResult> UpdateParticipantRole(UpdateParticipantRoleRequest request)
    {
        var participant = await repository.FindParticipantByIdAsync(request.Id);

        if (participant is null)
            return NotFound();

        participant.ChangeRole(request.Role);

        return Ok();
    }
}
