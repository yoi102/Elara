using ApiClients.Abstractions;
using ApiClients.Abstractions.Models.Responses;
using Frontend.Shared.Exceptions;
using Services.Abstractions;
using Services.Abstractions.PersonalSpaceServices;

namespace Services.PersonalSpaceServices;

internal class PersonalSpaceProfileService : IPersonalSpaceProfileService
{
    private readonly IPersonalSpaceProfileApiClient personalSpaceProfileApiClient;

    public PersonalSpaceProfileService(IPersonalSpaceProfileApiClient personalSpaceProfileApiClient)
    {
        this.personalSpaceProfileApiClient = personalSpaceProfileApiClient;
    }

    public async Task<ApiServiceResult<UserProfileData>> GetCurrentUserProfileAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await personalSpaceProfileApiClient.GetCurrentUserProfileAsync(cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            return new ApiServiceResult<UserProfileData>()
            {
                IsSuccessful = true,
                ResultData = response.ResponseData
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ApiServiceResult<UserProfileData>()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }

            throw new ApiResponseException();
        }
    }

    public async Task<ApiServiceResult<UserProfileData>> GetUserProfileAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await personalSpaceProfileApiClient.GetUserProfileAsync(id, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            return new ApiServiceResult<UserProfileData>()
            {
                IsSuccessful = true,
                ResultData = response.ResponseData
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ApiServiceResult<UserProfileData>()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }

            throw new ApiResponseException();
        }
    }

    public async Task<ApiServiceResult> UpdateUserProfileAsync(UserProfileData userProfileData, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await personalSpaceProfileApiClient.UpdateUserProfileAsync(userProfileData, cancellationToken);

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
