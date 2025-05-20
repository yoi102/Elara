using DomainCommons.EntityStronglyIds;
using PersonalSpaceService.WebAPI.Controllers.ProfileController.Models.Responses;

namespace PersonalSpaceService.WebAPI.Controllers.ContactController.Models.Responses;

public record ContactResponse
{
    public required ContactId Id { get; set; }
    public required ContactUserInfoResponse ContactUserInfo { get; set; }
    public required string Remark { get; set; }
    public required DateTimeOffset ContactedAt { get; set; }
}

public record ContactUserInfoResponse
{
    public required UserId UserId { get; set; }
    public required string DisplayName { get; set; }
    public required UploadedItemResponse? Avatar { get; set; }
}


