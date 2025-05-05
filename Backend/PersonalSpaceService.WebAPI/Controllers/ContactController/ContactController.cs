using ASPNETCore;
using DomainCommons.EntityStronglyIds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalSpaceService.Domain.Entities;
using PersonalSpaceService.Domain.Interfaces;
using PersonalSpaceService.WebAPI.Controllers.ContactController.Requests;

namespace PersonalSpaceService.WebAPI.Controllers.ContactController;

[Authorize]
[ApiController]
[Route("api/contacts")]
public class ContactController : AuthorizedUserController
{
    private readonly ILogger<ContactController> logger;
    private readonly IPersonalSpaceRepository repository;

    public ContactController(ILogger<ContactController> logger,
        IPersonalSpaceRepository repository)
    {
        this.logger = logger;
        this.repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<Contact[]>> GetContacts()
    {
        var contacts = await repository.AllUserContactsAsync(GetCurrentUserId());
        return Ok(contacts);
    }

    [HttpDelete("{contactId}")]
    public async Task<ActionResult> DeleteContact([RequiredGuidStronglyId] ContactId contactId)
    {
        await repository.DeleteContactAsync(contactId);

        return Ok();
    }

    [HttpGet("{contactId}")]
    public async Task<ActionResult<Contact>> GetContact([RequiredGuidStronglyId] ContactId contactId)
    {
        var contact = await repository.FindContactByContactIdAsync(contactId);
        if (contact == null)
            return NotFound();

        return Ok(contact);
    }

    [HttpPatch("{contactId}")]
    public async Task<ActionResult<Contact>> UpdateContactInfo([RequiredGuidStronglyId] ContactId contactId, UpdateContactInfoRequest request)
    {
        var contact = await repository.UpdateContactInfoAsync(contactId, request.Remark);
        if (contact is null)
        {
            return NotFound();
        }
        return Ok(contact);
    }
}
