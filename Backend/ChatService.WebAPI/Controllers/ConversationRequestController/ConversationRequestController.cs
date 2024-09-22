using ASPNETCore;
using ChatService.Domain;
using ChatService.Infrastructure;
using DomainCommons.EntityStronglyIds;
using DomainCommons.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatService.WebAPI.Controllers.ConversationRequestController;

[Authorize]
[ApiController]
[Route("api/conversation-request")]
public class ConversationRequestController : AuthorizedUserController
{
    private readonly ChatServiceDbContext dbContext;
    private readonly ChatDomainService domainService;
    private readonly ILogger<ConversationRequestController> logger;
    private readonly IChatServiceRepository repository;

    public ConversationRequestController(ILogger<ConversationRequestController> logger,
                                         ChatServiceDbContext dbContext,
                                         IChatServiceRepository repository,
                                         ChatDomainService domainService)
    {
        this.logger = logger;
        this.dbContext = dbContext;
        this.repository = repository;
        this.domainService = domainService;
    }

    [HttpPatch("accept")]
    public async Task<IActionResult> AcceptConversationRequest(ConversationRequestId id)
    {
        var request = await repository.FindConversationRequestByIdAsync(id);

        if (request is null)
        {
            return NotFound();
        }

        request.UpdateStatus(RequestStatus.Accepted);
        return Ok(request);
    }

    [HttpGet()]
    public async Task<IActionResult> GetConversationRequests()
    {
        var result = await repository.GetAllReceiverConversationRequestAsync(GetCurrentUserId());
        return Ok(result);
    }

    [HttpPatch("reject")]
    public async Task<IActionResult> RejectConversationRequest(ConversationRequestId id)
    {
        var request = await repository.FindConversationRequestByIdAsync(id);

        if (request is null)
        {
            return NotFound();
        }

        request.UpdateStatus(RequestStatus.Rejected);
        return Ok(request);
    }

    [HttpPost()]
    public async Task<IActionResult> SendConversationRequest(UserId receiverId, ConversationId conversationId)
    {
        if (dbContext.Participants.Any(x => x.ConversationId == conversationId && x.UserId == GetCurrentUserId()))
            return BadRequest();

        var request = await repository.CreateConversationRequestAsync(GetCurrentUserId(), receiverId, conversationId);
        return Ok(request);
    }
}
