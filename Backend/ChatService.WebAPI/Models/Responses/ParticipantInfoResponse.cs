using ChatService.Domain.Entities;
using DomainCommons.EntityStronglyIds;

namespace ChatService.WebAPI.Models.Responses;

public record ParticipantInfoResponse
{
    public required ParticipantId Id { get; set; }
    public required ConversationId ConversationId { get; set; }
    public required string Role { get; set; }
    public required UserInfoResponse UserInfo { get; set; }
}
