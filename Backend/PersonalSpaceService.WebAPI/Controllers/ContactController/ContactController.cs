using ASPNETCore;
using DomainCommons.EntityStronglyIds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalSpaceService.Domain.Interfaces;
using PersonalSpaceService.WebAPI.Controllers.ContactController.Requests;

namespace PersonalSpaceService.WebAPI.Controllers.ContactController
{
    [ApiController]
    [Route("api/contact")]
    public class ContactController : ControllerBase
    {
        private readonly ILogger<ContactController> _logger;
        private readonly IPersonalSpaceRepository personalSpaceRepository;

        public ContactController(ILogger<ContactController> logger, IPersonalSpaceRepository personalSpaceRepository)
        {
            _logger = logger;
            this.personalSpaceRepository = personalSpaceRepository;
        }



        [Authorize]
        [HttpPost]
        [Route("all-contact")]
        public ActionResult AllContact([RequiredGuidStronglyId] UserId userId)
        {



            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("get-contact")]
        public ActionResult GetContact([RequiredGuidStronglyId] ContactId contactId)
        {



            return Ok();
        }



        [Authorize]
        [HttpPost]
        [Route("add-contact")]
        public ActionResult AddContact([RequiredGuidStronglyId] UserId userId, [RequiredGuidStronglyId] UserId contactUserId)
        {



            return Ok();
        }

        [Authorize]
        [HttpPatch]
        [Route("update-contact-info")]
        public ActionResult UpdateContactInfo([RequiredGuidStronglyId] ContactId contactId, UpdateContactInfoRequest request)
        {



            return Ok();
        }


        [Authorize]
        [HttpDelete]
        [Route("delete-contact")]
        public ActionResult DeleteContact([RequiredGuidStronglyId] ContactId contactId)
        {



            return Ok();
        }


    }
}
