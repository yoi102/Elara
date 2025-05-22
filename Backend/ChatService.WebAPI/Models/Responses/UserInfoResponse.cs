using DomainCommons.EntityStronglyIds;

namespace ChatService.WebAPI.Models.Responses;

public record UserInfoResponse
{
    public required UserId UserId { get; set; }
    public required string DisplayName { get; set; }
    public required UploadedItemResponse? Avatar { get; set; }
}
