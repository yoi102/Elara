using ApiClients.Abstractions.PersonalSpaceApiClient.ContactRequest;
using ApiClients.Abstractions.PersonalSpaceApiClient.ContactRequest.Responses;
using Frontend.Shared.Exceptions;
using Services.Abstractions.PersonalSpaceServices;
using Services.Abstractions.Results;

namespace Services.PersonalSpaceServices;

internal class PersonalSpaceContactRequestService : IPersonalSpaceContactRequestService
{
    private readonly IPersonalSpaceContactRequestApiClient personalSpaceContactRequestApiClient;

    public PersonalSpaceContactRequestService(IPersonalSpaceContactRequestApiClient personalSpaceContactRequestApiClient)
    {
        this.personalSpaceContactRequestApiClient = personalSpaceContactRequestApiClient;
    }

    public async Task<ApiServiceResult> AcceptContactRequestAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await personalSpaceContactRequestApiClient.AcceptContactRequestAsync(id, cancellationToken);

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
            else if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
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

    public async Task<ApiServiceResult<ContactRequestData[]>> GetReceivedContactRequestsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await personalSpaceContactRequestApiClient.GetReceivedContactRequestsAsync(cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            return new ApiServiceResult<ContactRequestData[]>()
            {
                IsSuccessful = true,
                ResultData = response.ResponseData
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ApiServiceResult<ContactRequestData[]>()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }
            else if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new ApiServiceResult<ContactRequestData[]>()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }
            throw new ApiResponseException();
        }
    }

    public async Task<ApiServiceResult> RejectContactRequestAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await personalSpaceContactRequestApiClient.RejectContactRequestAsync(id, cancellationToken);

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
            else if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
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

    public async Task<ApiServiceResult> SendContactRequestAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await personalSpaceContactRequestApiClient.SendContactRequestAsync(id, cancellationToken);

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
            else if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
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
