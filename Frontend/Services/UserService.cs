using ApiClients.Abstractions.UserApiClient;
using ApiClients.Abstractions.UserIdentityApiClient.Responses;
using Frontend.Shared.Exceptions;
using Services.Abstractions;
using Services.Abstractions.Results;

namespace Services;

public class UserService : IUserService
{
    private readonly IUserApiClient userApiClient;

    public UserService(IUserApiClient userApiClient)
    {
        this.userApiClient = userApiClient;
    }

    public async Task<ApiServiceResult> DeleteAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await userApiClient.DeleteAsync(cancellationToken);
            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
            return new ApiServiceResult()
            {
                IsSuccessful = true
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ApiServiceResult()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }
            else if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new ForceLogoutException();
            }

            throw new ApiResponseException();
        }
    }

    public async Task<ApiServiceResult<UserInfo>> GetUserInfoAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await userApiClient.GetUserInfoAsync(cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            return new ApiServiceResult<UserInfo>()
            {
                IsSuccessful = true,
                ResultData = response.ResponseData,
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ApiServiceResult<UserInfo>()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }
            else if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new ForceLogoutException();
            }

            throw new ApiResponseException();
        }
    }
}
