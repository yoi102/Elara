using ApiClients.Abstractions.FileApiClient;
using ApiClients.Abstractions.FileApiClient.Responses;
using ApiClients.Abstractions.UserIdentityApiClient.Responses;
using Frontend.Shared.Exceptions;
using RestSharp;

namespace ApiClients.Clients;

public class FileApiClient : IFileApiClient
{
    private const string serviceUri = "/FileApiClient/api/uploader";
    private readonly ITokenRefreshingRestClient client;

    public FileApiClient(ITokenRefreshingRestClient client)
    {
        this.client = client;
    }

    public async Task UploadFileAsync(params string[] filePaths)
    {
        var restRequest = new RestRequest
        {
            Resource = serviceUri + "files",
            Method = Method.Post
        };
        foreach (var filePath in filePaths)
        {
            restRequest.AddFile("files", filePath);
        }

        var response = await client.ExecuteWithAutoRefreshAsync(restRequest);

        if (!response.IsSuccessful)
            throw new Exception($"Upload failed: {response.StatusCode} - {response.ErrorMessage}");//TODO
    }

    public async Task UploadFileAsync(params Stream[] streams)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + "files",
            Method = Method.Post
        };
        foreach (var stream in streams)
        {
            request.AddFile("files", () => stream, Guid.NewGuid().ToString());
        }

        var response = await client.ExecuteWithAutoRefreshAsync(request);

        if (!response.IsSuccessful)
            throw new Exception($"Upload failed: {response.StatusCode} - {response.ErrorMessage}");//TODO
    }

    public async Task<FileItemResponse> GetFileItemAsync(Guid itemId)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{itemId}",
            Method = Method.Get
        };

        var response = await client.ExecuteWithAutoRefreshAsync(request);

        if (string.IsNullOrEmpty(response.Content))
            throw new ApiResponseException();

        if (!response.IsSuccessful || string.IsNullOrWhiteSpace(response.Content))
            throw new ApiResponseException();

        var data = JsonUtils.DeserializeInsensitive<FileItemData>(response.Content);

        if (data is null)
            throw new ApiResponseException();

        return new FileItemResponse() { IsSuccessful = true, StatusCode = response.StatusCode, ResponseData = data };
    }

    public async Task DownloadFileAsync(FileItemData fileItem, string path)
    {
        //此处会创建 client 进行下载、不会阻塞
        using var client = new RestClient();
        var fileDownloadRequest = new RestRequest(fileItem.RemoteUrl, Method.Get);
        var downloadResponse = await client.ExecuteAsync(fileDownloadRequest);

        if (!downloadResponse.IsSuccessful || downloadResponse.RawBytes == null)
            throw new ApiResponseException();

        var savePath = Path.Combine(path, fileItem.Filename);

        await File.WriteAllBytesAsync(savePath, downloadResponse.RawBytes);
    }
}
