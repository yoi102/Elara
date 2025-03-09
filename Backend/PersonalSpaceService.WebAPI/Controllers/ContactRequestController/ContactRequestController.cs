using DomainCommons.EntityStronglyIds;
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
    private readonly UserId userId;

    public ContactRequestController(ILogger<ContactRequestController> logger,
        IPersonalSpaceRepository repository)
    {
        this.logger = logger;
        this.repository = repository;
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
        return Ok(request);
    }
}
