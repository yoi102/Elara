using ASPNETCore;
using ChatService.Domain;
using ChatService.Infrastructure;
using ChatService.WebAPI.Controllers.ConversationRequestController.Requests;
using DomainCommons.EntityStronglyIds;
using DomainCommons.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    [HttpPatch("{id}/accept")]
    public async Task<IActionResult> AcceptConversationRequest(ConversationRequestId id)
    {
        var request = await repository.FindConversationRequestByIdAsync(id);

        if (request is null)
        {
            return NotFound();
        }
        var conversation = await repository.FindConversationByIdAsync(request.ConversationId);
        if (conversation is null)
        {
            return NotFound();
        }

        await dbContext.Participants.AddAsync(new Domain.Entities.Participant(request.ConversationId, request.ReceiverId, request.Role));

        request.UpdateStatus(RequestStatus.Accepted);
        return Ok(request);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> FindById(ConversationRequestId id)
    {
        var conversationRequest = await repository.FindConversationRequestByIdAsync(id);

        if (conversationRequest is null) NotFound();

        return Ok(conversationRequest);
    }

    [HttpGet()]
    public async Task<IActionResult> GetConversationRequests()
    {
        var result = await repository.GetAllReceiverConversationRequestAsync(GetCurrentUserId());
        return Ok(result);
    }

    [HttpPatch("{id}/reject")]
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
    public async Task<IActionResult> SendConversationRequest(ConversationRequestRequest request)
    {
        if (dbContext.Participants.Any(x => x.ConversationId == request.ConversationId && x.UserId == GetCurrentUserId()))
            return Conflict();

        var conversationRequest = await repository.CreateConversationRequestAsync(GetCurrentUserId(), request.ReceiverId, request.ConversationId, request.Role);
        return Created(nameof(FindById), conversationRequest);
    }
}
