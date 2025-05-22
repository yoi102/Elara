using ApiClients.Abstractions;
using ApiClients.Abstractions.Models.Responses;
using Frontend.Shared.Exceptions;
using Services.Abstractions;

namespace Services;

public class FileService : IFileService
{
    private readonly IFileApiClient fileApiClient;

    public FileService(IFileApiClient fileApiClient)
    {
        this.fileApiClient = fileApiClient;
    }

    public async Task<ApiServiceResult<UploadedItemData>> GetFileItemAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await fileApiClient.GetFileItemAsync(id, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
            return new ApiServiceResult<UploadedItemData>()
            {
                IsSuccessful = true,
                ResultData = response.ResponseData
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ApiServiceResult<UploadedItemData>()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }
            if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new ApiServiceResult<UploadedItemData>()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = false
                };
            }
            throw new ApiResponseException();
        }
    }

    public async Task<ApiServiceResult<UploadedItemData[]>> GetFileItemsAsync(Guid[] fileIds, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await fileApiClient.GetFileItemsAsync(fileIds, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
            return new ApiServiceResult<UploadedItemData[]>()
            {
                IsSuccessful = true,
                ResultData = response.ResponseData
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ApiServiceResult<UploadedItemData[]>()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }
            if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new ApiServiceResult<UploadedItemData[]>()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = false
                };
            }
            throw new ApiResponseException();
        }
    }

    public async Task<ApiServiceResult> UploadFileAsync(string[] filePaths, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await fileApiClient.UploadFileAsync(filePaths, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
            return new ApiServiceResult()
            {
                IsSuccessful = true
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ApiServiceResult()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }
            throw new ApiResponseException();
        }
    }

    public async Task<ApiServiceResult> UploadFileAsync(Stream[] streams, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await fileApiClient.UploadFileAsync(streams, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
            return new ApiServiceResult()
            {
                IsSuccessful = true
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ApiServiceResult()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }
            throw new ApiResponseException();
        }
    }
}
