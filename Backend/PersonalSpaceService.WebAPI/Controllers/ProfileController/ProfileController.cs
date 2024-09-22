using ASPNETCore;
using DomainCommons.EntityStronglyIds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalSpaceService.Domain;
using PersonalSpaceService.Domain.Interfaces;
using PersonalSpaceService.WebAPI.Controllers.ProfileController.Requsets;

namespace PersonalSpaceService.WebAPI.Controllers.ProfileController;

[ApiController]
[Authorize]
[Route("api/profile")]
public class ProfileController : AuthorizedUserController
{
    private readonly PersonalSpaceDomainService domainService;
    private readonly ILogger<ProfileController> logger;
    private readonly IPersonalSpaceRepository repository;

    public ProfileController(ILogger<ProfileController> logger,
        IPersonalSpaceRepository repository,
        PersonalSpaceDomainService domainService)
    {
        this.logger = logger;
        this.repository = repository;
        this.domainService = domainService;
    }

    [HttpPatch]
    public async Task<IActionResult> UserUpdateProfile(UpdateProfileRequest request)
    {
        var profile = await domainService.UpdateProfileAsync(GetCurrentUserId(), request.DisplayName, request.Avatar);

        if (profile is null)
        {
            return NotFound();
        }
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetUserProfile(UserId userId)
    {
        var profile = await repository.FindProfileByUserIdAsync(userId);

        if (profile is null)
        {
            return NotFound();
        }

        return Ok(profile);
    }
}
