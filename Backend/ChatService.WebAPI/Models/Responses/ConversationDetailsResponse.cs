using DomainCommons.EntityStronglyIds;

namespace ChatService.WebAPI.Models.Responses;

public record ConversationDetailsResponse
{
    public required ConversationId Id { get; set; }
    public required string Name { get; set; }
    public required bool IsGroup { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    public required MessageWithReplyMessageResponse[] Messages { get; set; }
    public required ParticipantInfoResponse[] Participants { get; set; }
}
