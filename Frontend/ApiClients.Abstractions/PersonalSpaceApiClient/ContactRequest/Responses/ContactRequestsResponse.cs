using ApiClients.Abstractions.PersonalSpaceApiClient.Profile.Responses;
using Frontend.Shared;

namespace ApiClients.Abstractions.PersonalSpaceApiClient.ContactRequest.Responses;

public record ContactRequestsResponse : ApiResponse<ContactRequestData[]>;
public record ContactRequestResponse : ApiResponse<ContactRequestData>;

public record ContactRequestData
{
    public required Guid Id { get; set; }
    public required SenderUserInfoData SenderInfo { get; set; }
    public required RequestStatus Status { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
}

public record SenderUserInfoData
{
    public required Guid UserId { get; set; }
    public required string DisplayName { get; set; }
    public required UploadedItemData? Avatar { get; set; }
}
