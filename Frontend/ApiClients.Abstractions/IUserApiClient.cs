using ApiClients.Abstractions.Models;
using ApiClients.Abstractions.Models.Responses;

namespace ApiClients.Abstractions;

public interface IUserApiClient
{
    Task<ApiResponse> DeleteAsync(CancellationToken cancellationToken = default);

    Task<ApiResponse<AccountInfoData>> GetUserInfoAsync(CancellationToken cancellationToken = default);
}
