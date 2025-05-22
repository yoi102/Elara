using DomainCommons.EntityStronglyIds;
using DomainCommons.Enums;

namespace ChatService.WebAPI.Models.Responses;

public record ConversationRequestResponse
{
    public required ConversationRequestId Id { get; set; }
    public required ConversationInfoResponse ConversationInfo { get; set; }
    public required UserInfoResponse SenderUserInfo { get; set; }
    public required string Role { get; set; }
    public required RequestStatus Status { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    public required DateTimeOffset? UpdatedAt { get; set; }
}

