using ApiClients.Abstractions.Models.Responses;

namespace Services.Abstractions.PersonalSpaceServices;

public interface IPersonalSpaceProfileService
{
    Task<ApiServiceResult<UserProfileData>> GetCurrentUserProfileAsync(CancellationToken cancellationToken = default);

    Task<ApiServiceResult<UserProfileData>> GetUserProfileAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiServiceResult> UpdateUserProfileAsync(UserProfileData userProfileData, CancellationToken cancellationToken = default);
}
