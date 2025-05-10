using ApiClients.Abstractions.FileApiClient.Responses;

namespace ApiClients.Abstractions.FileApiClient;

public interface IFileApiClient
{
    Task<ApiResponse> UploadFileAsync(string[] filePaths, CancellationToken cancellationToken = default);

    Task<ApiResponse> UploadFileAsync(Stream[] streams, CancellationToken cancellationToken = default);

    Task<FileItemResponse> GetFileItemAsync(Guid id, CancellationToken cancellationToken = default);

    Task<FileItemsResponse> GetFileItemsAsync(Guid[] fileIds, CancellationToken cancellationToken = default);
}
