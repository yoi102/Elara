using ASPNETCore;
using DomainCommons.EntityStronglyIds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalSpaceService.Domain.Entities;
using PersonalSpaceService.Domain.Interfaces;
using PersonalSpaceService.WebAPI.Controllers.ContactController.Models.Requests;
using PersonalSpaceService.WebAPI.Controllers.ContactController.Models.Responses;
using PersonalSpaceService.WebAPI.Controllers.ProfileController.Models.Responses;
using UploadedItem;

namespace PersonalSpaceService.WebAPI.Controllers.ContactController;

[Authorize]
[ApiController]
[Route("api/contacts")]
public class ContactController : AuthorizedUserController
{
    private readonly ILogger<ContactController> logger;
    private readonly IPersonalSpaceRepository repository;
    private readonly UploadedItemService.UploadedItemServiceClient uploadedItemServiceClient;

    public ContactController(ILogger<ContactController> logger,
        IPersonalSpaceRepository repository,
        UploadedItemService.UploadedItemServiceClient uploadedItemServiceClient)
    {
        this.logger = logger;
        this.repository = repository;
        this.uploadedItemServiceClient = uploadedItemServiceClient;
    }

    [HttpGet]
    public async Task<ActionResult<Contact[]>> GetContacts()
    {
        var contacts = await repository.AllUserContactsAsync(GetCurrentUserId());

        var responses = new List<ContactResponse>();
        foreach (var contact in contacts)
        {
            var profile = await repository.FindProfileByUserIdAsync(contact.ContactUserId);
            if (profile is null)
                continue;

            var avatar = default(UploadedItemResponse);
            if (profile.AvatarItemId is not null)
            {
                var uploadedItemReply = uploadedItemServiceClient.GetUploadedItem(new UploadedItemRequest() { Id = profile.AvatarItemId.ToString() });

                if (uploadedItemReply.Id == profile.AvatarItemId.ToString())
                {
                    avatar = new UploadedItemResponse()
                    {
                        Id = UploadedItemId.Parse(uploadedItemReply.Id),
                        Filename = uploadedItemReply.Filename,
                        FileType = uploadedItemReply.FileType,
                        FileSizeInBytes = uploadedItemReply.FileSizeInBytes,
                        FileSHA256Hash = uploadedItemReply.FileSha256Hash,
                        Url = new Uri(uploadedItemReply.Url),
                        UploadedAt = DateTimeOffset.Parse(uploadedItemReply.UploadedAt)
                    };
                }
            }

            var response = new ContactResponse
            {
                Id = contact.Id,
                ContactUserInfo = new ContactUserInfoResponse
                {
                    UserId = contact.ContactUserId,
                    DisplayName = profile.DisplayName,
                    Avatar = avatar
                },
                Remark = contact.Remark,
                ContactedAt = contact.CreatedAt
            };
            responses.Add(response);
        }

        return Ok(responses);
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
        if (contact is null)
            return NotFound();

        var profile = await repository.FindProfileByUserIdAsync(contact.ContactUserId);
        if (profile is null)
            return NotFound();

        var avatar = default(UploadedItemResponse);
        if (profile.AvatarItemId is not null)
        {
            var uploadedItemReply = uploadedItemServiceClient.GetUploadedItem(new UploadedItemRequest() { Id = profile.AvatarItemId.ToString() });

            if (uploadedItemReply.Id == profile.AvatarItemId.ToString())
            {
                avatar = new UploadedItemResponse()
                {
                    Id = UploadedItemId.Parse(uploadedItemReply.Id),
                    Filename = uploadedItemReply.Filename,
                    FileType = uploadedItemReply.FileType,
                    FileSizeInBytes = uploadedItemReply.FileSizeInBytes,
                    FileSHA256Hash = uploadedItemReply.FileSha256Hash,
                    Url = new Uri(uploadedItemReply.Url),
                    UploadedAt = DateTimeOffset.Parse(uploadedItemReply.UploadedAt)
                };
            }
        }

        var response = new ContactResponse
        {
            Id = contact.Id,
            ContactUserInfo = new ContactUserInfoResponse
            {
                UserId = contact.ContactUserId,
                DisplayName = profile.DisplayName,
                Avatar = avatar
            },
            Remark = contact.Remark,
            ContactedAt = contact.CreatedAt
        };

        return Ok(response);
    }

    [HttpPatch("{contactId}")]
    public async Task<ActionResult<Contact>> UpdateContactInfo([RequiredGuidStronglyId] ContactId contactId, UpdateContactInfoRequest request)
    {
        var contact = await repository.UpdateContactInfoAsync(contactId, request.Remark);
        if (contact is null)
        {
            return NotFound();
        }
        return Ok();
    }
}
