using ASPNETCore;
using DomainCommons.EntityStronglyIds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PersonalSpaceService.Domain;
using PersonalSpaceService.Domain.Entities;
using PersonalSpaceService.Domain.Interfaces;
using PersonalSpaceService.WebAPI.Controllers.ContactController.Requests;
using System.Net.Http;
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
        private readonly HttpClient httpClient;
        private readonly IPersonalSpaceRepository repository;
        private readonly PersonalSpaceDomainService domainService;

        public ContactController(ILogger<ContactController> logger, HttpClient httpClient, IPersonalSpaceRepository repository, PersonalSpaceDomainService domainService)
        {
            this.logger = logger;
            this.httpClient = httpClient;
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

        [HttpGet]
        public async Task<ActionResult<Contact[]>> AllContacts()
        {
            var contacts = await repository.AllUserContactsAsync(userId);
            return Ok(contacts);
        }

        [HttpGet]
        [Route("id/{contactId}")]
        public async Task<ActionResult<Contact>> GetContact([RequiredGuidStronglyId] ContactId contactId)
        {
            var contact = await repository.FindContactByContactIdAsync(contactId);
            if (contact == null)
                return NotFound();

            return Ok(contact);
        }



        [HttpPost]
        [Route("user-id/{contactUserId}")]
        public async Task<ActionResult<Contact>> AddContact([RequiredGuidStronglyId] UserId contactUserId)
        {
            var response = await httpClient.GetAsync($"XXXXX{contactUserId}");

            //var contact = new Contact(userId,);




            return Ok();
        }

        [HttpPatch]
        public ActionResult UpdateContactInfo([RequiredGuidStronglyId] ContactId contactId, UpdateContactInfoRequest request)
        {



            return Ok();
        }


        [HttpDelete]
        public ActionResult DeleteContact([RequiredGuidStronglyId] ContactId contactId)
        {



            return Ok();
        }


    }
}
