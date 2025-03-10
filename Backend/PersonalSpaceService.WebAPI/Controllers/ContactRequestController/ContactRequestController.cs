using DomainCommons.EntityStronglyIds;
using Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalSpaceService.Domain.Entities;
using PersonalSpaceService.Domain.Interfaces;
using System.Security.Claims;

namespace PersonalSpaceService.WebAPI.Controllers.ContactRequestController;

[Authorize]
[Route("api/contact-requests")]
[ApiController]
public class ContactRequestController : ControllerBase
{
    private readonly ILogger<ContactRequestController> logger;
    private readonly IPersonalSpaceRepository repository;
    private readonly Identifier.IdentifierClient identifierClient;
    private readonly UserId userId;

    public ContactRequestController(ILogger<ContactRequestController> logger,
        IPersonalSpaceRepository repository,
        Identifier.IdentifierClient identifierClient)
    {
        this.logger = logger;
        this.repository = repository;
        this.identifierClient = identifierClient;
        var stringUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(stringUserId))
        {
            throw new UnauthorizedAccessException();
        }
        if (!UserId.TryParse(stringUserId, out userId))
        {
            throw new UnauthorizedAccessException();
        }
    }

    [HttpPatch("accept")]
    public async Task<IActionResult> AcceptContactRequest(ContactRequestId id)
    {
        var request = await repository.FindContactRequestByIdAsync(id);

        if (request is null)
        {
            return NotFound();
        }

        var sender = new UserInfoRequest() { Id = request.SenderId.ToString() };
        var receiver = new UserInfoRequest() { Id = request.SenderId.ToString() };
        var senderInfo = await identifierClient.GetUserInfoAsync(sender);
        var receiverInfo = await identifierClient.GetUserInfoAsync(receiver);
        if (senderInfo?.UserName is null)
        {
            return NotFound();
        }
        await repository.AddContactAsync(userId, request.SenderId, senderInfo.UserName);

        if (receiverInfo?.UserName is null)
        {
            return NotFound();
        }

        await repository.AddContactAsync(request.SenderId, userId, receiverInfo.UserName);
        //todo:notify

        request.UpdateStatus(ContactRequestStatus.Accepted);
        return Ok(request);
    }

    [HttpGet()]
    public async Task<IActionResult> GetContactRequests()
    {
        var result = await repository.AllContactRequestByReceiverIdAsync(userId);
        return Ok(result);
    }

    [HttpPatch("reject")]
    public async Task<IActionResult> RejectContactRequest(ContactRequestId id)
    {
        var request = await repository.FindContactRequestByIdAsync(id);

        if (request is null)
        {
            return NotFound();
        }

        request.UpdateStatus(ContactRequestStatus.Rejected);
        //todo:notify
        return Ok(request);
    }

    [HttpPost()]
    public async Task<IActionResult> SendContactRequest(UserId receiverId)
    {
        var request = await repository.FindContactRequestByUserIdAsync(userId, receiverId);

        if (request is not null)
        {
            request = await repository.CreateContactRequestAsync(userId, receiverId);
        }
        //todo:notify
        return Ok(request);
    }
}
