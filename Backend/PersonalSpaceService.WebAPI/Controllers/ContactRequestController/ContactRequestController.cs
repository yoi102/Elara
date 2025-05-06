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

    [HttpPatch("{id}/accept")]
    public async Task<IActionResult> AcceptContactRequest(ContactRequestId id)
    {
        var request = await repository.FindContactRequestByIdAsync(id);

        if (request is null)
        {
            return NotFound();
        }

        var sender = new UserInfoRequest() { Id = request.SenderId.ToString() };
        var receiver = new UserInfoRequest() { Id = request.ReceiverId.ToString() };
        var senderInfo = await identifierClient.GetUserInfoAsync(sender);//也许不应该用GRPC的
        var receiverInfo = await identifierClient.GetUserInfoAsync(receiver);//也许不应该用GRPC的
        if (senderInfo?.UserName is null)
        {
            return NotFound();
        }
        await repository.AddContactAsync(request.ReceiverId, request.SenderId, senderInfo.UserName);

        if (receiverInfo?.UserName is null)
        {
            return NotFound();
        }

        await repository.AddContactAsync(request.SenderId, request.ReceiverId, receiverInfo.UserName);

        request.UpdateStatus(RequestStatus.Accepted);
        return Ok(request);
    }

    [HttpGet()]
    public async Task<IActionResult> GetReceivedContactRequests()
    {
        var result = await repository.AllContactRequestByReceiverIdAsync(GetCurrentUserId());
        return Ok(result);
    }

    [HttpPatch("{id}/reject")]
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

    [HttpPost("{receiverId}")]
    public async Task<IActionResult> SendContactRequest(UserId receiverId)
    {
        var receiver = new UserInfoRequest() { Id = receiverId.ToString() };
        var receiverInfo = await identifierClient.GetUserInfoAsync(receiver);//也许不应该用GRPC的
        if (receiverInfo?.UserName is null)
        {
            return NotFound();
        }
        var request = await repository.CreateContactRequestAsync(GetCurrentUserId(), receiverId);
        return Ok(request);
    }
}
