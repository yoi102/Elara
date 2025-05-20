using DomainCommons.EntityStronglyIds;

namespace PersonalSpaceService.WebAPI.Controllers.ProfileController.Models.Responses;

public record UserProfileResponse
{
    public required ProfileId Id { get; set; }
    public required AccountInfoResponse AccountInfo { get; set; }
    public required string DisplayName { get; set; }
    public required UploadedItemResponse? Avatar { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
}

public record AccountInfoResponse
{
    public required UserId Id { get; set; }
    public required string Name { get; set; }
    public required string? Email { get; set; }
    public required string? PhoneNumber { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
}

public record UploadedItemResponse
{
    public required UploadedItemId Id { get; set; }
    public required long FileSizeInBytes { get; set; }
    public required string Filename { get; set; }
    public required string FileType { get; set; }
    public required string FileSHA256Hash { get; set; }
    public required Uri Url { get; set; }
    public required DateTimeOffset UploadedAt { get; set; }
}
