using ASPNETCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalSpaceService.Domain;
using PersonalSpaceService.Domain.Interfaces;
using PersonalSpaceService.WebAPI.Controllers.ProfileController.Requests;

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
    public async Task<IActionResult> UpdateUserProfile(UpdateProfileRequest request)
    {
        var profile = await domainService.UpdateProfileAsync(GetCurrentUserId(), request.DisplayName, request.AvatarItemId);

        if (profile is null)
        {
            return NotFound();
        }
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetUserProfile()
    {
        var profile = await repository.FindProfileByUserIdAsync(GetCurrentUserId());

        if (profile is null)
        {
            return NotFound();
        }

        return Ok(profile);
    }
}
