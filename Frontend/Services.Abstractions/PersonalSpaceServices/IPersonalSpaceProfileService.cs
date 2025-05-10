using ApiClients.Abstractions.PersonalSpaceApiClient.Profile.Responses;
using Services.Abstractions.Results;

namespace Services.Abstractions.PersonalSpaceServices;

public interface IPersonalSpaceProfileService
{
    Task<ApiServiceResult<UserProfileData>> GetCurrentUserProfileAsync(CancellationToken cancellationToken = default);

    Task<ApiServiceResult<UserProfileData>> GetUserProfileAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiServiceResult> UpdateUserProfileAsync(UserProfileData userProfileData, CancellationToken cancellationToken = default);
}
