namespace ApiClients.Abstractions.Models.Responses;

public record UserProfileData
{
    public required Guid Id { get; set; }
    public required AccountInfoData AccountInfo { get; set; }
    public required string DisplayName { get; set; }
    public required UploadedItemData? Avatar { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
}
