using ApiClients.Abstractions.PersonalSpaceApiClient.Profile.Responses;

namespace ApiClients.Abstractions.PersonalSpaceApiClient.Profile;

public interface IPersonalSpaceProfileApiClient
{
    Task<UserProfileResponse> GetCurrentUserProfileAsync(CancellationToken cancellationToken = default);

    Task<UserProfileResponse> GetUserProfileAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiResponse> UpdateUserProfileAsync(UserProfileData userProfileData, CancellationToken cancellationToken = default);
}
