namespace Services.Abstractions.Results.Data;
public record ParticipantData
{
    public required Guid UserId { get; init; }
    public required string Name { get; init; }
    public required string Role { get; init; }
    public required UploadedItemData Avatar { get; init; }


}
