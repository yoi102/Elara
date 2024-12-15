using DomainCommons.EntityStronglyIds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalSpaceService.Domain;
using PersonalSpaceService.Domain.Entities;
using PersonalSpaceService.Domain.Interfaces;
using PersonalSpaceService.WebAPI.Controllers.ProfileController.Requsets;
using System.Security.Claims;

namespace PersonalSpaceService.WebAPI.Controllers.ProfileController
{
    [ApiController]
    [Authorize]
    [Route("api/profile")]
    public class ProfileController : ControllerBase
    {
        private readonly PersonalSpaceDomainService domainService;
        private readonly ILogger<ProfileController> logger;
        private readonly IPersonalSpaceRepository repository;
        private readonly UserId userId;

        public ProfileController(ILogger<ProfileController> logger,
            IPersonalSpaceRepository repository,
            PersonalSpaceDomainService domainService)
        {
            this.logger = logger;
            this.repository = repository;
            this.domainService = domainService;
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

        [HttpPatch]
        public async Task<ActionResult<Profile>> UserUpdateProfile(UpdateProfileRequest request)
        {
            var profile = await domainService.UpdateProfileAsync(userId, request.DisplayName, request.Avatar);

            if (profile is null)
            {
                return NotFound();
            }

            return Ok(profile);
        }
    }
}