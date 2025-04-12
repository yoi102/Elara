using ASPNETCore;
using DomainCommons.EntityStronglyIds;
using DomainCommons.Enums;
using Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalSpaceService.Domain.Interfaces;

namespace PersonalSpaceService.WebAPI.Controllers.ContactRequestController;

[Authorize]
[Route("api/contact-requests")]
[ApiController]
public class ContactRequestController : AuthorizedUserController
{
    private readonly Identifier.IdentifierClient identifierClient;
    private readonly ILogger<ContactRequestController> logger;
    private readonly IPersonalSpaceRepository repository;

    public ContactRequestController(ILogger<ContactRequestController> logger,
        IPersonalSpaceRepository repository,
        Identifier.IdentifierClient identifierClient)
    {
        this.logger = logger;
        this.repository = repository;
        this.identifierClient = identifierClient;
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
        await repository.AddContactAsync(GetCurrentUserId(), request.SenderId, senderInfo.UserName);

        if (receiverInfo?.UserName is null)
        {
            return NotFound();
        }

        await repository.AddContactAsync(request.SenderId, GetCurrentUserId(), receiverInfo.UserName);

        request.UpdateStatus(RequestStatus.Accepted);
        return Ok(request);
    }

    [HttpGet()]
    public async Task<IActionResult> GetContactRequests()
    {
        var result = await repository.AllContactRequestByReceiverIdAsync(GetCurrentUserId());
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

        request.UpdateStatus(RequestStatus.Rejected);
        return Ok(request);
    }

    [HttpPost()]
    public async Task<IActionResult> SendContactRequest(UserId receiverId)
    {
        var request = await repository.CreateContactRequestAsync(GetCurrentUserId(), receiverId);
        return Ok(request);
    }
}
