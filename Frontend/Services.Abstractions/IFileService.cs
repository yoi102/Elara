using ApiClients.Abstractions.Models.Responses;

namespace Services.Abstractions;

public interface IFileService
{
    Task<UploadedItemData?> GetFileItemAsync(Guid id, CancellationToken cancellationToken = default);

    Task<UploadedItemData[]> GetFileItemsAsync(Guid[] fileIds, CancellationToken cancellationToken = default);

    Task UploadFileAsync(string[] filePaths, CancellationToken cancellationToken = default);

    Task UploadFileAsync(Stream[] streams, CancellationToken cancellationToken = default);
}
