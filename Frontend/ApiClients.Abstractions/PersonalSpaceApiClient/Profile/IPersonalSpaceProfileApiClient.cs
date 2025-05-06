using ApiClients.Abstractions.PersonalSpaceApiClient.Profile.Responses;

namespace ApiClients.Abstractions.PersonalSpaceApiClient.Profile;

public interface IPersonalSpaceProfileApiClient
{
    Task<UserProfileResponse> GetUserProfileAsync(CancellationToken cancellationToken = default);

    Task<ApiResponse> UpdateUserProfileAsync(UserProfileData userProfileData, CancellationToken cancellationToken = default);
}
