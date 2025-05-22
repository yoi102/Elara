using ASPNETCore;
using DomainCommons.EntityStronglyIds;
using Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalSpaceService.Domain;
using PersonalSpaceService.Domain.Interfaces;
using PersonalSpaceService.WebAPI.Controllers.ProfileController.Models.Requests;
using PersonalSpaceService.WebAPI.Controllers.ProfileController.Models.Responses;
using UploadedItem;

namespace PersonalSpaceService.WebAPI.Controllers.ProfileController;

[ApiController]
[Authorize]
[Route("api/profile")]
public class ProfileController : AuthorizedUserController
{
    private readonly PersonalSpaceDomainService domainService;
    private readonly Identifier.IdentifierClient identifierClient;
    private readonly UploadedItemService.UploadedItemServiceClient uploadedItemServiceClient;
    private readonly ILogger<ProfileController> logger;
    private readonly IPersonalSpaceRepository repository;

    public ProfileController(ILogger<ProfileController> logger,
        IPersonalSpaceRepository repository,
        PersonalSpaceDomainService domainService,
        Identifier.IdentifierClient identifierClient,
        UploadedItemService.UploadedItemServiceClient uploadedItemServiceClient)
    {
        this.logger = logger;
        this.repository = repository;
        this.domainService = domainService;
        this.identifierClient = identifierClient;
        this.uploadedItemServiceClient = uploadedItemServiceClient;
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateUserProfile(UpdateProfileRequest request)
    {
        var profile = await domainService.UpdateProfileAsync(GetCurrentUserId(), request.DisplayName, request.AvatarItemId);

        if (profile is null)
        {
            return NotFound();
        }
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetCurrentUserProfile()
    {
        return await GetUserProfile(GetCurrentUserId());
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserProfile(UserId userId)
    {
        var profile = await repository.FindProfileByUserIdAsync(userId);

        if (profile is null)
            return NotFound();


        var userInfoReply = await identifierClient.GetAccountInfoAsync(new AccountInfoRequest() { Id = profile.UserId.ToString() });

        if (userInfoReply.Id != profile.UserId.ToString())
            return NotFound();

        var userInfo = new AccountInfoResponse()
        {
            Id = UserId.Parse(userInfoReply.Id),
            Name = userInfoReply.Name,
            Email = userInfoReply.Email,
            PhoneNumber = userInfoReply.PhoneNumber,
            CreatedAt = DateTimeOffset.Parse(userInfoReply.CreatedAt)
        };
        var avatar = default(UploadedItemResponse);
        if (profile.AvatarItemId is not null)
        {
            var uploadedItemReply = await uploadedItemServiceClient.GetUploadedItemAsync(new UploadedItemRequest() { Id = profile.AvatarItemId.ToString() });

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
        var response = new UserProfileResponse()
        {
            Id = profile.Id,
            AccountInfo = userInfo,
            DisplayName = profile.DisplayName,
            Avatar = avatar,
            CreatedAt = profile.CreatedAt,
        };

        return Ok(response);
    }
}
