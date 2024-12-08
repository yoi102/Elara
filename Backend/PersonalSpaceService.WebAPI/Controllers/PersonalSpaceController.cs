using ASPNETCore;
using DomainCommons.EntityStronglyIds;
using Microsoft.AspNetCore.Mvc;
using PersonalSpaceService.WebAPI.Controllers.Request;

namespace PersonalSpaceService.WebAPI.Controllers
{
    [ApiController]
    [Route("api/personal")]
    public class PersonalSpaceController : ControllerBase
    {
        private readonly ILogger<PersonalSpaceController> _logger;

        public PersonalSpaceController(ILogger<PersonalSpaceController> logger)
        {
            _logger = logger;
        }




        //[Authorize]
        [HttpPatch]
        [Route("update-profile")]
        public ActionResult UpdateProfile([RequiredGuidStronglyId] UserId userId, UpdateProfileRequest updateProfileRequest)
        {






            return Ok();
        }






    }
}