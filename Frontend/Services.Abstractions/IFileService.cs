using ApiClients.Abstractions.FileApiClient.Responses;
using Services.Abstractions.Results;

namespace Services.Abstractions;

public interface IFileService
{
    Task<ApiServiceResult<FileItemData>> GetFileItemAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<FileItemData[]>> GetFileItemsAsync(Guid[] fileIds, CancellationToken cancellationToken = default);

    Task<ApiServiceResult> UploadFileAsync(string[] filePaths, CancellationToken cancellationToken = default);

    Task<ApiServiceResult> UploadFileAsync(Stream[] streams, CancellationToken cancellationToken = default);
}
