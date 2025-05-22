namespace ApiClients.Abstractions.Models.Responses;
public record ConversationDetailsData
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required bool IsGroup { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    public required MessageWithReplyMessageData[] Messages { get; set; }
    public required ParticipantData[] Participants { get; set; }
}
