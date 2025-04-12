using ASPNETCore;
using DomainCommons.EntityStronglyIds;
using Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalSpaceService.Domain;
using PersonalSpaceService.Domain.Interfaces;
using PersonalSpaceService.WebAPI.Controllers.ProfileController.Requsets;
using PersonalSpaceService.WebAPI.Controllers.ProfileController.Results;
using System.Security.Claims;

namespace PersonalSpaceService.WebAPI.Controllers.ProfileController;

[ApiController]
[Authorize]
[Route("api/profile")]
public class ProfileController : AuthorizedUserController
{
    private readonly PersonalSpaceDomainService domainService;
    private readonly Identifier.IdentifierClient identifierClient;
    private readonly UploadedItem.UploadedItem.UploadedItemClient uploadedItemClient;
    private readonly ILogger<ProfileController> logger;
    private readonly IPersonalSpaceRepository repository;

    public ProfileController(ILogger<ProfileController> logger,
        IPersonalSpaceRepository repository,
        PersonalSpaceDomainService domainService,
        Identifier.IdentifierClient identifierClient,
        UploadedItem.UploadedItem.UploadedItemClient uploadedItemClient)
    {
        this.logger = logger;
        this.repository = repository;
        this.domainService = domainService;
        this.identifierClient = identifierClient;
        this.uploadedItemClient = uploadedItemClient;
        var stringUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    [HttpPatch]
    public async Task<ActionResult> UserUpdateProfile(UpdateProfileRequest request)
    {
        var profile = await domainService.UpdateProfileAsync(GetCurrentUserId(), request.DisplayName, request.Avatar);

        if (profile is null)
        {
            return NotFound();
        }
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<ProfileResult>> GetUserProfile(UserId userId)
    {
        var profile = await repository.FindProfileByUserIdAsync(userId);

        if (profile is null)
        {
            return NotFound();
        }
        var user = new UserInfoRequest() { Id = userId.ToString() };

        var userInfo = await identifierClient.GetUserInfoAsync(user);

        var uploadedItemRequest = new UploadedItem.UploadedItemRequest();
        uploadedItemRequest.Ids.Add(profile.Avatar.ToString());

        var uploadedItemReply = await uploadedItemClient.GetUploadedItemsAsync(uploadedItemRequest);

        var result = new ProfileResult()
        {
            UserId = userId,
            Name = userInfo.UserName,
            DisplayName = profile.DisplayName,
            Email = userInfo.Email,
            PhoneNumber = userInfo.PhoneNumber,
            AvatarUrl = uploadedItemReply.Urls.FirstOrDefault()
        };

        return Ok(result);
    }
}
