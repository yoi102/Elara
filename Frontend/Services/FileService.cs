using ApiClients.Abstractions;
using ApiClients.Abstractions.Models.Responses;
using Services.Abstractions;

namespace Services;

public class FileService : IFileService
{
    private readonly IFileApiClient fileApiClient;

    public FileService(IFileApiClient fileApiClient)
    {
        this.fileApiClient = fileApiClient;
    }

    public async Task<UploadedItemData?> GetFileItemAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await fileApiClient.GetFileItemAsync(id, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
            return response.ResponseData;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<UploadedItemData[]> GetFileItemsAsync(Guid[] fileIds, CancellationToken cancellationToken = default)
    {
        var response = await fileApiClient.GetFileItemsAsync(fileIds, cancellationToken);

        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
        return response.ResponseData;
    }

    public async Task UploadFileAsync(string[] filePaths, CancellationToken cancellationToken = default)
    {
        var response = await fileApiClient.UploadFileAsync(filePaths, cancellationToken);

        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
    }

    public async Task UploadFileAsync(Stream[] streams, CancellationToken cancellationToken = default)
    {
        var response = await fileApiClient.UploadFileAsync(streams, cancellationToken);

        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
    }
}
