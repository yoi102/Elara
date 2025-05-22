using ApiClients.Abstractions.Models;
using ApiClients.Abstractions.Models.Responses;

namespace ApiClients.Abstractions;

public interface IPersonalSpaceProfileApiClient
{
    Task<ApiResponse<UserProfileData>> GetCurrentUserProfileAsync(CancellationToken cancellationToken = default);

    Task<ApiResponse<UserProfileData>> GetUserProfileAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiResponse> UpdateUserProfileAsync(UserProfileData userProfileData, CancellationToken cancellationToken = default);
}
