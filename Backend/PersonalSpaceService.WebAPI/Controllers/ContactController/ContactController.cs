using ASPNETCore;
using DomainCommons.EntityStronglyIds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PersonalSpaceService.Domain;
using PersonalSpaceService.Domain.Entities;
using PersonalSpaceService.Domain.Interfaces;
using PersonalSpaceService.WebAPI.Controllers.ContactController.Requests;
using System.Security.Claims;

namespace PersonalSpaceService.WebAPI.Controllers.ContactController
{
    [Authorize]
    [ApiController]
    [Route("api/contact")]
    public class ContactController : ControllerBase
    {
        private readonly UserId userId;
        private readonly ILogger<ContactController> logger;
        private readonly IPersonalSpaceRepository repository;
        private readonly PersonalSpaceDomainService domainService;

        public ContactController(ILogger<ContactController> logger, IPersonalSpaceRepository repository, PersonalSpaceDomainService domainService)
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



        [HttpPost]
        [Route("all-contacts")]
        public async Task<ActionResult<Contact[]>> AllContacts()
        {
            var contacts = await repository.AllUserContactsAsync(userId);
            return Ok(contacts);
        }

        [HttpPost]
        [Route("get-contact")]
        public ActionResult GetContact([RequiredGuidStronglyId] ContactId contactId)
        {



            return Ok();
        }



        [HttpPost]
        [Route("add-contact")]
        public ActionResult AddContact([RequiredGuidStronglyId] UserId userId, [RequiredGuidStronglyId] UserId contactUserId)
        {



            return Ok();
        }

        [HttpPatch]
        [Route("update-contact-info")]
        public ActionResult UpdateContactInfo([RequiredGuidStronglyId] ContactId contactId, UpdateContactInfoRequest request)
        {



            return Ok();
        }


        [HttpDelete]
        [Route("delete-contact")]
        public ActionResult DeleteContact([RequiredGuidStronglyId] ContactId contactId)
        {



            return Ok();
        }


    }
}
