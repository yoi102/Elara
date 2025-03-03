using DomainCommons.EntityStronglyIds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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













}
