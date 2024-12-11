using ASPNETCore;
using DomainCommons.EntityStronglyIds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalSpaceService.WebAPI.Controllers.ProfileController.Requsets;

namespace PersonalSpaceService.WebAPI.Controllers.ProfileController
{
    [ApiController]
    [Authorize]
    [Route("api/personal")]
    public class ProfileController : ControllerBase
    {
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(ILogger<ProfileController> logger)
        {
            _logger = logger;
        }




        [HttpPatch]
        [Route("update-profile")]
        public ActionResult UserUpdateProfile([RequiredGuidStronglyId] UserId userId, UpdateProfileRequest request)
        {




            return Ok();
        }

      


    }
}