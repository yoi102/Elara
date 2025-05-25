using ApiClients.Abstractions.Models.Responses;

namespace Services.Abstractions.PersonalSpaceServices;

public interface IPersonalSpaceProfileService
{
    Task<UserProfileData> GetCurrentUserProfileAsync(CancellationToken cancellationToken = default);

    Task<UserProfileData?> GetUserProfileAsync(Guid id, CancellationToken cancellationToken = default);

    Task UpdateUserProfileAsync(UserProfileData userProfileData, CancellationToken cancellationToken = default);
}
