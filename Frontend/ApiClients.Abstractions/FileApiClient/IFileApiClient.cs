using ApiClients.Abstractions.FileApiClient.Responses;

namespace ApiClients.Abstractions.FileApiClient;

public interface IFileApiClient
{
    Task UploadFileAsync(params string[] filePaths);

    Task UploadFileAsync(params Stream[] streams);

    Task<FileItemResponse> GetFileItemAsync(Guid itemId);

    Task DownloadFileAsync(FileItemData fileItem, string path);
}
