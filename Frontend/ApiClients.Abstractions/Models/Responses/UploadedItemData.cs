namespace ApiClients.Abstractions.Models.Responses;
public record UploadedItemData
{
    public required Guid Id { get; set; }
    public required long FileSizeInBytes { get; set; }
    public required string Filename { get; set; }
    public required string FileType { get; set; }
    public required string FileSHA256Hash { get; set; }
    public required Uri Url { get; set; }
    public required DateTimeOffset UploadedAt { get; set; }
}
