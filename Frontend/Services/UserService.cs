using ApiClients.Abstractions;
using ApiClients.Abstractions.Models.Responses;
using Frontend.Shared.Exceptions;
using Services.Abstractions;

namespace Services;

public class UserService : IUserService
{
    private readonly IUserApiClient userApiClient;

    public UserService(IUserApiClient userApiClient)
    {
        this.userApiClient = userApiClient;
    }

    public async Task DeleteAsync(CancellationToken cancellationToken = default)
    {
        var response = await userApiClient.DeleteAsync(cancellationToken);
        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
    }

    public async Task<AccountInfoData> GetUserInfoAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await userApiClient.GetUserInfoAsync(cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            return response.ResponseData;
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                  ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new ForceLogoutException();
            }

            throw new ApiResponseException();
        }
    }
}
