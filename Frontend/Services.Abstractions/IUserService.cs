using ApiClients.Abstractions.Models.Responses;

namespace Services.Abstractions;

public interface IUserService
{
    Task DeleteAsync(CancellationToken cancellationToken = default);

    Task<AccountInfoData> GetUserInfoAsync(CancellationToken cancellationToken = default);
}
