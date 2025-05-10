namespace Services.Abstractions.Results.Data;
public record MessageSenderData
{
    public required Guid SenderId { get; init; }
    public required string Name { get; init; }
    public required UploadedItemData Avatar { get; init; }
}
