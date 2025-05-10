using ApiClients.Abstractions;
using ApiClients.Abstractions.ChatApiClient.Participant;
using ApiClients.Abstractions.FileApiClient;
using ApiClients.Abstractions.FileApiClient.Responses;
using Frontend.Shared.Exceptions;
using Services.Abstractions;
using Services.Abstractions.Results;
using System.IO;
using System.Threading;

namespace Services;

public class FileService : IFileService
{
    private readonly IFileApiClient fileApiClient;

    public FileService(IFileApiClient fileApiClient)
    {
        this.fileApiClient = fileApiClient;
    }

    public async Task<ApiServiceResult<FileItemData>> GetFileItemAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await fileApiClient.GetFileItemAsync(id, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
            return new ApiServiceResult<FileItemData>()
            {
                IsSuccessful = true,
                ResultData = response.ResponseData
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ApiServiceResult<FileItemData>()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }
            if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new ApiServiceResult<FileItemData>()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = false
                };
            }
            throw new ApiResponseException();
        }
    }

    public async Task<ApiServiceResult<FileItemData[]>> GetFileItemsAsync(Guid[] fileIds, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await fileApiClient.GetFileItemsAsync(fileIds, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
            return new ApiServiceResult<FileItemData[]>()
            {
                IsSuccessful = true,
                ResultData = response.ResponseData
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ApiServiceResult<FileItemData[]>()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }
            if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new ApiServiceResult<FileItemData[]>()
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
