using Services.Abstractions.Results;
using Services.Abstractions.Results.Data;

namespace Services.Abstractions;
public interface IUserProfileService
{
    Task<ApiServiceResult<UserInfoData>> GetUserInfoDataById(Guid userId, CancellationToken cancellationToken = default);
}
