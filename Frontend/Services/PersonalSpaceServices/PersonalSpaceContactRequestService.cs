using ApiClients.Abstractions.PersonalSpaceApiClient.ContactRequest;
using Frontend.Shared.Exceptions;
using Services.Abstractions.PersonalSpaceServices;
using Services.Abstractions.Results;
using Services.Abstractions.Results.Results;

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

    public async Task<ContactRequestsResult> GetReceivedContactRequestsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await personalSpaceContactRequestApiClient.GetReceivedContactRequestsAsync(cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            var data = response.ResponseData.Select(x => new ContactRequestResultData(x.Id, x.SenderId, x.ReceiverId, (Abstractions.Results.Results.RequestStatus)((int)x.Status), x.CreatedAt, x.UpdatedAt)).ToArray();
            return new ContactRequestsResult()
            {
                IsSuccessful = true,
                ResultData = data
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ContactRequestsResult()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }
            else if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new ContactRequestsResult()
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
