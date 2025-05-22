namespace ApiClients.Abstractions.Models.Responses;
public record UserInfoData
{
    public required Guid UserId { get; set; }
    public required string DisplayName { get; set; }
    public required UploadedItemData? Avatar { get; set; }
}
