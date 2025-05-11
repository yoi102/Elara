namespace Services.Abstractions.Results.Data;
public record ConversationData
{
    public required Guid Id { get; init; }

    public required bool IsGroup { get; init; }

    public required string Name { get; init; }

    public required MessageData[] Messages { get; init; }

    public required ParticipantData[] Participants { get; init; }
}
