using ApiClients.Abstractions.Models;
using ApiClients.Abstractions.Models.Responses;

namespace ApiClients.Abstractions;

public interface IFileApiClient
{
    Task<ApiResponse> UploadFileAsync(string[] filePaths, CancellationToken cancellationToken = default);

    Task<ApiResponse> UploadFileAsync(Stream[] streams, CancellationToken cancellationToken = default);

    Task<ApiResponse<UploadedItemData>> GetFileItemAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiResponse<UploadedItemData[]>> GetFileItemsAsync(Guid[] fileIds, CancellationToken cancellationToken = default);
}
