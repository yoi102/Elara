using ApiClients.Abstractions;
using ApiClients.Abstractions.Models.Responses;
using Services.Abstractions.PersonalSpaceServices;

namespace Services.PersonalSpaceServices;

internal class PersonalSpaceProfileService : IPersonalSpaceProfileService
{
    private readonly IPersonalSpaceProfileApiClient personalSpaceProfileApiClient;

    public PersonalSpaceProfileService(IPersonalSpaceProfileApiClient personalSpaceProfileApiClient)
    {
        this.personalSpaceProfileApiClient = personalSpaceProfileApiClient;
    }

    public async Task<UserProfileData> GetCurrentUserProfileAsync(CancellationToken cancellationToken = default)
    {
        var response = await personalSpaceProfileApiClient.GetCurrentUserProfileAsync(cancellationToken);

        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

        return response.ResponseData;
    }

    public async Task<UserProfileData?> GetUserProfileAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await personalSpaceProfileApiClient.GetUserProfileAsync(id, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            return response.ResponseData;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task UpdateUserProfileAsync(UserProfileData userProfileData, CancellationToken cancellationToken = default)
    {
        var response = await personalSpaceProfileApiClient.UpdateUserProfileAsync(userProfileData, cancellationToken);

        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
    }
}
