namespace ApiClients.Abstractions.FileApiClient.Responses;
public record FileItemResponse : ApiResponse<FileItemData>;
public record FileItemData
{
    public required Guid Id { get; init; }

    public required DateTimeOffset CreatedAt { get; init; }

    public required long FileSizeInBytes { get; init; }

    public required string Filename { get; init; }

    public required string FileType { get; init; }

    public required string FileSHA256Hash { get; init; }

    public required Uri RemoteUrl { get; init; }

    public required Uri BackupUrl { get; init; }
}
