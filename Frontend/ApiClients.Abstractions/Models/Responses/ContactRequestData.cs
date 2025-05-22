using Frontend.Shared;

namespace ApiClients.Abstractions.Models.Responses;

public record ContactRequestData
{
    public required Guid Id { get; set; }
    public required UserInfoData SenderInfo { get; set; }
    public required RequestStatus Status { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
}
