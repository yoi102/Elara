﻿using ASPNETCore;
using DomainCommons.EntityStronglyIds;
using Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalSpaceService.Domain.Entities;
using PersonalSpaceService.Domain.Interfaces;
using PersonalSpaceService.WebAPI.Controllers.ContactController.Requests;
using System.Security.Claims;

namespace PersonalSpaceService.WebAPI.Controllers.ContactController;

[Authorize]
[ApiController]
[Route("api/contacts")]
public class ContactController : ControllerBase
{
    private readonly ILogger<ContactController> logger;
    private readonly IPersonalSpaceRepository repository;
    private readonly UserId userId;

    public ContactController(ILogger<ContactController> logger,
        IPersonalSpaceRepository repository)
    {
        this.logger = logger;
        this.repository = repository;
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

    [HttpDelete]
    public async Task<ActionResult> DeleteContact([RequiredGuidStronglyId] ContactId contactId)
    {
        await repository.DeleteContactAsync(contactId);

        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<Contact>> GetContact([RequiredGuidStronglyId] ContactId contactId)
    {
        var contact = await repository.FindContactByContactIdAsync(contactId);
        if (contact == null)
            return NotFound();

        return Ok(contact);
    }

    [HttpPatch]
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
