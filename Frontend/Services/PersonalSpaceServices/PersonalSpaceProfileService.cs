using ApiClients.Abstractions.PersonalSpaceApiClient.Profile;
using ApiClients.Abstractions.PersonalSpaceApiClient.Profile.Responses;
using Frontend.Shared.Exceptions;
using Services.Abstractions.PersonalSpaceServices;
using Services.Abstractions.Results;
using Services.Abstractions.Results.Results;

namespace Services.PersonalSpaceServices;

internal class PersonalSpaceProfileService : IPersonalSpaceProfileService
{
    private readonly IPersonalSpaceProfileApiClient personalSpaceProfileApiClient;

    public PersonalSpaceProfileService(IPersonalSpaceProfileApiClient personalSpaceProfileApiClient)
    {
        this.personalSpaceProfileApiClient = personalSpaceProfileApiClient;
    }

    public async Task<UserProfileResult> GetUserProfileAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await personalSpaceProfileApiClient.GetUserProfileAsync(cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            var data = new UserProfileResultData(response.ResponseData.Id, response.ResponseData.DisplayName, response.ResponseData.AvatarItemId, response.ResponseData.CreatedAt, response.ResponseData.UpdatedAt);

            return new UserProfileResult()
            {
                IsSuccessful = true,
                ResultData = data
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new UserProfileResult()
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
