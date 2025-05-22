using ASPNETCore;
using ChatService.Domain;
using ChatService.Infrastructure;
using ChatService.WebAPI.Models.Requests;
using ChatService.WebAPI.Models.Responses;
using ChatService.WebAPI.Services;
using DomainCommons.EntityStronglyIds;
using DomainCommons.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatService.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/conversation-request")]
public class ConversationRequestController : AuthorizedUserController
{
    private readonly ChatServiceDbContext dbContext;
    private readonly ChatDomainService domainService;
    private readonly IMessageQueryService messageQueryService;
    private readonly ILogger<ConversationRequestController> logger;
    private readonly IChatServiceRepository repository;

    public ConversationRequestController(ILogger<ConversationRequestController> logger,
                                         ChatServiceDbContext dbContext,
                                         IChatServiceRepository repository,
                                         ChatDomainService domainService,
                                         IMessageQueryService messageQueryService)
    {
        this.logger = logger;
        this.dbContext = dbContext;
        this.repository = repository;
        this.domainService = domainService;
        this.messageQueryService = messageQueryService;
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
        return Ok();
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
        var conversationRequests = await repository.GetAllReceiverConversationRequestAsync(GetCurrentUserId());

        var result = new List<ConversationRequestResponse>();
        foreach (var conversationRequest in conversationRequests)
        {
            var conversation = await repository.FindConversationByIdAsync(conversationRequest.ConversationId);
            if (conversation is null)
                continue;
            var receiverInfo = await messageQueryService.GetUserInfoAsync(conversationRequest.ReceiverId);
            if (receiverInfo is null)
                continue;

            var conversationRequestResponse = new ConversationRequestResponse()
            {
                Id = conversationRequest.Id,
                SenderUserInfo = receiverInfo,
                Role = conversationRequest.Role,
                Status = conversationRequest.Status,
                CreatedAt = conversationRequest.CreatedAt,
                UpdatedAt = conversationRequest.UpdatedAt,
                ConversationInfo = new ConversationInfoResponse()
                {
                    Id = conversation.Id,
                    Name = conversation.Name,
                    CreatedAt = conversation.CreatedAt,
                    IsGroup = conversation.IsGroup,
                }
            };
            result.Add(conversationRequestResponse);
        }

        return Ok(result);
    }

    [HttpPatch("{id}/reject")]
    public async Task<IActionResult> RejectConversationRequest(ConversationRequestId id)
    {
        var request = await repository.FindConversationRequestByIdAsync(id);

        if (request is null)
            return NotFound();

        request.UpdateStatus(RequestStatus.Rejected);
        return Ok();
    }

    [HttpPost()]
    public async Task<IActionResult> SendConversationRequest(ConversationRequestRequest request)
    {
        var conversation = await repository.FindConversationByIdAsync(request.ConversationId);
        if (conversation is null)
            return NotFound("Conversation not found");

        if (dbContext.Participants.Any(x => x.ConversationId == request.ConversationId && x.UserId == GetCurrentUserId()))
            return Conflict();

        var receiverInfo = await messageQueryService.GetUserInfoAsync(request.ReceiverId);
        if (receiverInfo is null)
            return NotFound("Receiver not found");

        var conversationRequest = await repository.CreateConversationRequestAsync(GetCurrentUserId(), request.ReceiverId, request.ConversationId, request.Role);

        var conversationRequestResponse = new ConversationRequestResponse()
        {
            Id = conversationRequest.Id,
            SenderUserInfo = receiverInfo,
            Role = conversationRequest.Role,
            Status = conversationRequest.Status,
            CreatedAt = conversationRequest.CreatedAt,
            UpdatedAt = conversationRequest.UpdatedAt,
            ConversationInfo = new ConversationInfoResponse()
            {
                Id = conversation.Id,
                Name = conversation.Name,
                CreatedAt = conversation.CreatedAt,
                IsGroup = conversation.IsGroup,
            }
        };

        return Created(nameof(FindById), conversationRequestResponse);
    }
}
