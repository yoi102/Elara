using ApiClients.Abstractions.Models.Responses;

namespace Services.Abstractions;

public interface IUserService
{
    Task<ApiServiceResult> DeleteAsync(CancellationToken cancellationToken = default);

    Task<ApiServiceResult<AccountInfoData>> GetUserInfoAsync(CancellationToken cancellationToken = default);
}
