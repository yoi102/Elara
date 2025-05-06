using ApiClients.Abstractions.PersonalSpaceApiClient.Profile.Responses;
using Services.Abstractions.Results;
using Services.Abstractions.Results.Results;

namespace Services.Abstractions.PersonalSpaceServices;

public interface IPersonalSpaceProfileService
{
    Task<UserProfileResult> GetUserProfileAsync(CancellationToken cancellationToken = default);

    Task<ApiServiceResult> UpdateUserProfileAsync(UserProfileData userProfileData, CancellationToken cancellationToken = default);
}
