using ApiClients.Abstractions.PersonalSpaceApiClient.Contact;
using Frontend.Shared.Exceptions;
using Services.Abstractions.PersonalSpaceServices;
using Services.Abstractions.Results;
using Services.Abstractions.Results.Results;

namespace Services.PersonalSpaceServices;

internal class PersonalSpaceContactService : IPersonalSpaceContactService
{
    private readonly IPersonalSpaceContactApiClient personalSpaceContactApiClient;

    public PersonalSpaceContactService(IPersonalSpaceContactApiClient personalSpaceContactApiClient)
    {
        this.personalSpaceContactApiClient = personalSpaceContactApiClient;
    }

    public async Task<ApiServiceResult> DeleteContactAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await personalSpaceContactApiClient.DeleteContactAsync(id, cancellationToken);

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

    public async Task<ContactsResult> GetContactsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await personalSpaceContactApiClient.GetContactsAsync(cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
            var data = response.ResponseData.Select(x => new ContactsResultData(x.Id, x.ContactId, x.Remark, x.CreatedAt, x.UpdatedAt)).ToArray();
            return new ContactsResult()
            {
                IsSuccessful = true,
                ResultData = data
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ContactsResult()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }

            throw new ApiResponseException();
        }
    }

    public async Task<ApiServiceResult> UpdateContactInfoAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await personalSpaceContactApiClient.UpdateContactInfoAsync(id, cancellationToken);

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
