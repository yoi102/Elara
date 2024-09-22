namespace ApiClients.Abstractions.FileApiClient.Responses;
public record FileItemResponse
{
    public required Guid Id { get; set; }

    public required DateTimeOffset CreatedAt { get; set; }

    public required long FileSizeInBytes { get; set; }

    public required string Filename { get; set; }

    public required string FileType { get; set; }

    public required string FileSHA256Hash { get; set; }

    public required Uri RemoteUrl { get; set; }

    public required Uri BackupUrl { get; set; }
}
