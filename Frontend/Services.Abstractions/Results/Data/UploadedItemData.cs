namespace Services.Abstractions.Results.Data;
public record UploadedItemData
{
    public required Guid Id { get; init; }
    public required Uri Uri { get; init; }
}
