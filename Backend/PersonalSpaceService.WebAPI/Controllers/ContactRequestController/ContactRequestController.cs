using ASPNETCore;
using DomainCommons.EntityStronglyIds;
using DomainCommons.Enums;
using Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalSpaceService.Domain.Interfaces;
using PersonalSpaceService.WebAPI.Controllers.ContactRequestController.Models.Responses;
using PersonalSpaceService.WebAPI.Controllers.ProfileController.Models.Responses;
using UploadedItem;

namespace PersonalSpaceService.WebAPI.Controllers.ContactRequestController;

[Authorize]
[Route("api/contact-requests")]
[ApiController]
public class ContactRequestController : AuthorizedUserController
{
    private readonly Identifier.IdentifierClient identifierClient;
    private readonly UploadedItemService.UploadedItemServiceClient uploadedItemServiceClient;
    private readonly ILogger<ContactRequestController> logger;
    private readonly IPersonalSpaceRepository repository;

    public ContactRequestController(ILogger<ContactRequestController> logger,
        IPersonalSpaceRepository repository,
        Identifier.IdentifierClient identifierClient,
        UploadedItemService.UploadedItemServiceClient uploadedItemServiceClient)
    {
        this.logger = logger;
        this.repository = repository;
        this.identifierClient = identifierClient;
        this.uploadedItemServiceClient = uploadedItemServiceClient;
    }

    [HttpPatch("{id}/accept")]
    public async Task<IActionResult> AcceptContactRequest(ContactRequestId id)
    {
        var request = await repository.FindContactRequestByIdAsync(id);

        if (request is null)
        {
            return NotFound();
        }

        var sender = new AccountInfoRequest() { Id = request.SenderId.ToString() };
        var receiver = new AccountInfoRequest() { Id = request.ReceiverId.ToString() };
        var senderInfo = await identifierClient.GetAccountInfoAsync(sender);//也许不应该用GRPC的
        var receiverInfo = await identifierClient.GetAccountInfoAsync(receiver);//也许不应该用GRPC的
        if (senderInfo?.Name is null)
        {
            return NotFound();
        }
        await repository.AddContactAsync(request.ReceiverId, request.SenderId, senderInfo.Name);

        if (receiverInfo?.Name is null)
        {
            return NotFound();
        }

        await repository.AddContactAsync(request.SenderId, request.ReceiverId, receiverInfo.Name);

        request.UpdateStatus(RequestStatus.Accepted);
        return Ok();
    }

    [HttpGet()]
    public async Task<IActionResult> GetReceivedContactRequests()
    {
        var contactRequests = await repository.AllContactRequestByReceiverIdAsync(GetCurrentUserId());

        var contactRequestResponses = new List<ContactRequestResponse>();

        foreach (var contactRequest in contactRequests)
        {
            var senderProfile = await repository.FindProfileByUserIdAsync(contactRequest.SenderId);
            if (senderProfile is null)
                continue;

            var avatar = default(UploadedItemResponse);
            if (senderProfile.AvatarItemId is not null)
            {
                var uploadedItemReply = uploadedItemServiceClient.GetUploadedItem(new UploadedItemRequest() { Id = senderProfile.AvatarItemId.ToString() });

                if (uploadedItemReply.Id == senderProfile.AvatarItemId.ToString())
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

            var senderInfo = new SenderUserInfoResponse()
            {
                UserId = senderProfile.UserId,
                DisplayName = senderProfile.DisplayName,
                Avatar = avatar
            };
            var response = new ContactRequestResponse()
            {
                Id = contactRequest.Id,
                SenderInfo = senderInfo,
                CreatedAt = contactRequest.CreatedAt,
                Status = contactRequest.Status,
            };

            contactRequestResponses.Add(response);
        }
        return Ok(contactRequestResponses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetReceivedContactRequest(ContactRequestId id)
    {
        var contactRequest = await repository.FindContactRequestByIdAsync(id);
        if (contactRequest is null)
            return NotFound();

        var senderProfile = await repository.FindProfileByUserIdAsync(contactRequest.SenderId);
        if (senderProfile is null)
            return NotFound();

        var avatar = default(UploadedItemResponse);
        if (senderProfile.AvatarItemId is not null)
        {
            var uploadedItemReply = uploadedItemServiceClient.GetUploadedItem(new UploadedItemRequest() { Id = senderProfile.AvatarItemId.ToString() });

            if (uploadedItemReply.Id == senderProfile.AvatarItemId.ToString())
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

        var senderInfo = new SenderUserInfoResponse()
        {
            UserId = senderProfile.UserId,
            DisplayName = senderProfile.DisplayName,
            Avatar = avatar
        };
        var response = new ContactRequestResponse()
        {
            Id = contactRequest.Id,
            SenderInfo = senderInfo,
            CreatedAt = contactRequest.CreatedAt,
            Status = contactRequest.Status,
        };

        return Ok(response);
    }

    [HttpPatch("{id}/reject")]
    public async Task<IActionResult> RejectContactRequest(ContactRequestId id)
    {
        var request = await repository.FindContactRequestByIdAsync(id);

        if (request is null)
        {
            return NotFound();
        }

        request.UpdateStatus(RequestStatus.Rejected);
        return Ok();
    }

    [HttpPost("{receiverId}")]
    public async Task<IActionResult> SendContactRequest(UserId receiverId)
    {
        var receiver = new AccountInfoRequest() { Id = receiverId.ToString() };
        var receiverInfo = await identifierClient.GetAccountInfoAsync(receiver);
        if (receiverInfo?.Name is null)
        {
            return NotFound();
        }
        var request = await repository.CreateContactRequestAsync(GetCurrentUserId(), receiverId);
        return Ok();
    }
}
