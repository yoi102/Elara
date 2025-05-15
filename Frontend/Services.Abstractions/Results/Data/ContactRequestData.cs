using Frontend.Shared;

namespace Services.Abstractions.Results.Data;

public record ContactRequestData
{
    public required Guid Id { get; init; }
    public required UserInfoData Sender { get; init; }
    public required RequestStatus Status { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required DateTimeOffset? UpdatedAt { get; init; }
}
