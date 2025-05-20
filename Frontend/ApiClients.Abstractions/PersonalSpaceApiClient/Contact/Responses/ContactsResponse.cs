using ApiClients.Abstractions.PersonalSpaceApiClient.Profile.Responses;

namespace ApiClients.Abstractions.PersonalSpaceApiClient.Contact.Responses;

public record ContactsResponse : ApiResponse<ContactData[]>;

public record ContactData
{
    public required Guid Id { get; set; }
    public required ContactUserInfoData ContactUserInfo { get; set; }
    public required string Remark { get; set; }
    public required DateTimeOffset ContactedAt { get; set; }
}

public record ContactUserInfoData
{
    public required Guid UserId { get; set; }
    public required string DisplayName { get; set; }
    public required UploadedItemData? Avatar { get; set; }
}
