using ApiClients.Abstractions.UserIdentityApiClient;
using ApiClients.Abstractions.UserIdentityApiClient.Responses;
using DataProviders.Abstractions;
using Frontend.Shared.Exceptions;
using Services.Abstractions;
using Services.Abstractions.Results;

namespace Services;

public class UserIdentityService : IUserIdentityService
{
    private readonly IUserAgentProvider userAgentProvider;
    private readonly IUserDataProvider userDataProvider;
    private readonly IUserIdentityApiClient userIdentityApiClient;

    public UserIdentityService(IUserIdentityApiClient userIdentityApiClient, IUserAgentProvider userAgentProvider, IUserDataProvider userDataProvider)
    {
        this.userIdentityApiClient = userIdentityApiClient;
        this.userAgentProvider = userAgentProvider;
        this.userDataProvider = userDataProvider;
    }

    public async Task<ApiServiceResult> SignUpAsync(string name, string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await userIdentityApiClient.SignUpAsync(name, email, password, cancellationToken);
            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
            return new ApiServiceResult()
            {
                IsSuccessful = true,
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
            else if (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                return new ApiServiceResult()
                {
                    IsSuccessful = false,
                    ErrorMessage = "The username or email has already been taken.",
                };
            }
            else if (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return new ApiServiceResult()
                {
                    IsSuccessful = false,
                    ErrorMessage = "Creation failed.",
                };
            }

            throw new ApiResponseException();
        }
    }

    public async Task<ApiServiceResult<ResetCodeData>> GetResetCodeByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await userIdentityApiClient.GetResetCodeByEmailAsync(email, cancellationToken);
            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            return new ApiServiceResult<ResetCodeData>()
            {
                IsSuccessful = true,
                ResultData = response.ResponseData
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ApiServiceResult<ResetCodeData>()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }
            else if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new ApiServiceResult<ResetCodeData>()
                {
                    IsSuccessful = false,
                    ErrorMessage = "User does not exist.",
                };
            }

            throw new ApiResponseException();
        }
    }

    public async Task<ApiServiceResult<UserTokenData>> LoginByEmailAndPasswordAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await userIdentityApiClient.LoginByEmailAndPasswordAsync(email, password, userAgentProvider.GetUserAgent(), cancellationToken);
            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            userDataProvider.UpdateUser(response.ResponseData.UserId, response.ResponseData.UserName, response.ResponseData.RefreshToken);
            return new ApiServiceResult<UserTokenData>()
            {
                IsSuccessful = true,
                ResultData = response.ResponseData
            };
        }
        catch (HttpRequestException ex)
        {
            return HandleLoginResponse(ex);
        }
    }

    public async Task<ApiServiceResult<UserTokenData>> LoginByNameAndPasswordAsync(string name, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await userIdentityApiClient.LoginByNameAndPasswordAsync(name, password, userAgentProvider.GetUserAgent(), cancellationToken);
            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            userDataProvider.UpdateUser(response.ResponseData.UserId, response.ResponseData.UserName, response.ResponseData.RefreshToken);
            return new ApiServiceResult<UserTokenData>()
            {
                IsSuccessful = true,
                ResultData = response.ResponseData
            };
        }
        catch (HttpRequestException ex)
        {
            return HandleLoginResponse(ex);
        }
    }

    public async Task<ApiServiceResult<UserTokenData>> RefreshCurrentUserTokenAsync(CancellationToken cancellationToken = default)
    {
        if (userDataProvider.UserId == Guid.Empty || userDataProvider.RefreshToken is null)
            throw new ForceLogoutException();

        return await RefreshTokenAsync(userDataProvider.UserId, userDataProvider.RefreshToken, cancellationToken);
    }

    public async Task<ApiServiceResult<UserTokenData>> RefreshTokenAsync(Guid userId, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userAgent = userAgentProvider.GetUserAgent();

        try
        {
            var response = await userIdentityApiClient.RefreshTokenAsync(userId, refreshToken, userAgent, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            userDataProvider.UpdateRefreshToken(response.ResponseData.AccessToken, response.ResponseData.RefreshToken);

            return new ApiServiceResult<UserTokenData>()
            {
                IsSuccessful = true,
                ResultData = response.ResponseData
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null)
                return new ApiServiceResult<UserTokenData>()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };

            if (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return new ApiServiceResult<UserTokenData>()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                };
            }
            else if ((int)ex.StatusCode >= 500)
            {
                return new ApiServiceResult<UserTokenData>()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }

            throw new ApiResponseException();
        }
    }

    public async Task<ApiServiceResult> ResetPasswordWithResetCodeAsync(string email, string newPassword, string resetCode, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await userIdentityApiClient.ResetPasswordWithEmailCodeAsync(email, newPassword, resetCode, cancellationToken);
            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
            return new ApiServiceResult()
            {
                IsSuccessful = true,
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
            else if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new ApiServiceResult()
                {
                    IsSuccessful = false,
                    ErrorMessage = "User does not exist.",
                };
            }
            else if (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return new ApiServiceResult()
                {
                    IsSuccessful = false,
                    ErrorMessage = "Invalid verification code.",
                };
            }

            throw new ApiResponseException();
        }
    }

    private ApiServiceResult<UserTokenData> HandleLoginResponse(HttpRequestException ex)
    {
        if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
        {
            return new ApiServiceResult<UserTokenData>()
            {
                IsSuccessful = false,
                ErrorMessage = ex.Message,
                IsServerError = true
            };
        }
        else if (ex.StatusCode == System.Net.HttpStatusCode.NotFound ||
            ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return new ApiServiceResult<UserTokenData>()
            {
                IsSuccessful = false,
                //ErrorMessage = ex.Message,//"密码账号错误"
                ErrorMessage = "Incorrect username or password.",
            };
        }
        else if (ex.StatusCode == System.Net.HttpStatusCode.Forbidden)
        {
            return new ApiServiceResult<UserTokenData>()
            {
                IsSuccessful = false,
                //ErrorMessage = ex.Message,
                ErrorMessage = "Login is disabled.",
            };
        }
        throw new ApiResponseException();
    }
}
