using ApiClients.Abstractions.FileApiClient.Responses;

namespace Services.Abstractions.Results.Data;
public record UserInfoData
{
    public required Guid UserId { get; init; }
    public required string DisplayName { get; init; }
    public required FileItemData? Avatar { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required DateTimeOffset? UpdatedAt { get; init; }
}
