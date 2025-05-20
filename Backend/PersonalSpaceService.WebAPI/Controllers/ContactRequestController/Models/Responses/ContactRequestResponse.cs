using DomainCommons.EntityStronglyIds;
using DomainCommons.Enums;
using PersonalSpaceService.WebAPI.Controllers.ProfileController.Models.Responses;

namespace PersonalSpaceService.WebAPI.Controllers.ContactRequestController.Models.Responses;

public record ContactRequestResponse
{
    public required ContactRequestId Id { get; set; }
    public required SenderUserInfoResponse SenderInfo { get; set; }
    public required RequestStatus Status { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
}

public record SenderUserInfoResponse
{
    public required UserId UserId { get; set; }
    public required string DisplayName { get; set; }
    public required UploadedItemResponse? Avatar { get; set; }
}
