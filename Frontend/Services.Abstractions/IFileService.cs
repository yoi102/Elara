using ApiClients.Abstractions.Models.Responses;

namespace Services.Abstractions;

public interface IFileService
{
    Task<ApiServiceResult<UploadedItemData>> GetFileItemAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<UploadedItemData[]>> GetFileItemsAsync(Guid[] fileIds, CancellationToken cancellationToken = default);

    Task<ApiServiceResult> UploadFileAsync(string[] filePaths, CancellationToken cancellationToken = default);

    Task<ApiServiceResult> UploadFileAsync(Stream[] streams, CancellationToken cancellationToken = default);
}
