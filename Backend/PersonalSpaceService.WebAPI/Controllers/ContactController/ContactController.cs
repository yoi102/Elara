using ASPNETCore;
using DomainCommons.EntityStronglyIds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalSpaceService.Domain;
using PersonalSpaceService.Domain.Entities;
using PersonalSpaceService.Domain.Interfaces;
using PersonalSpaceService.WebAPI.Controllers.ContactController.Requests;

namespace PersonalSpaceService.WebAPI.Controllers.ContactController
{
    [ApiController]
    [Route("api/contact")]
    public class ContactController : ControllerBase
    {
        private readonly ILogger<ContactController> logger;
        private readonly IPersonalSpaceRepository repository;
        private readonly PersonalSpaceDomainService domainService;

        public ContactController(ILogger<ContactController> logger, IPersonalSpaceRepository repository, PersonalSpaceDomainService domainService)
        {
            this.logger = logger;
            this.repository = repository;
            this.domainService = domainService;
        }



        [Authorize]
        [HttpPost]
        [Route("all-contacts")]
        public async Task<ActionResult<Contact[]>> AllContacts([RequiredGuidStronglyId] UserId userId)
        {
            var contacts = await repository.AllUserContactsAsync(userId);
            return Ok(contacts);
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
