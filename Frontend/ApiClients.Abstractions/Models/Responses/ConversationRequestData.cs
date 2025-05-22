using Frontend.Shared;

namespace ApiClients.Abstractions.Models.Responses;

public record ConversationRequestData
{
    public required Guid Id { get; set; }
    public required ConversationInfoData ConversationInfo { get; set; }
    public required UserInfoData SenderUserInfo { get; set; }
    public required string Role { get; set; }
    public required RequestStatus Status { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    public required DateTimeOffset? UpdatedAt { get; set; }
}
