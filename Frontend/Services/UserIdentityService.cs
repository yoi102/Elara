using ApiClients.Abstractions;
using ApiClients.Abstractions.Models.Responses;
using DataProviders.Abstractions;
using Frontend.Shared.Exceptions;
using Services.Abstractions;

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

    public async Task SignUpAsync(string name, string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await userIdentityApiClient.SignUpAsync(name, email, password, cancellationToken);
            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
        {
            throw new HttpRequestException("The username or email has already been taken.", ex, ex.StatusCode);
        }
    }

    public async Task<ResetCodeData> GetResetCodeByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await userIdentityApiClient.GetResetCodeByEmailAsync(email, cancellationToken);
            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            return response.ResponseData;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new HttpRequestException("User does not exist.", ex, ex.StatusCode);
        }
    }

    public async Task<UserTokenData?> LoginByEmailAndPasswordAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await userIdentityApiClient.LoginByEmailAndPasswordAsync(email, password, userAgentProvider.GetUserAgent(), cancellationToken);
            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            userDataProvider.UpdateUser(response.ResponseData.UserId, response.ResponseData.UserName, response.ResponseData.RefreshToken);
            return response.ResponseData;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return null; // Login failed, return null to indicate failure
        }
    }

    public async Task<UserTokenData?> LoginByNameAndPasswordAsync(string name, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await userIdentityApiClient.LoginByNameAndPasswordAsync(name, password, userAgentProvider.GetUserAgent(), cancellationToken);
            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            userDataProvider.UpdateUser(response.ResponseData.UserId, response.ResponseData.UserName, response.ResponseData.RefreshToken);
            return response.ResponseData;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return null; // Login failed, return null to indicate failure
        }
    }

    public async Task<UserTokenData> RefreshCurrentUserTokenAsync(CancellationToken cancellationToken = default)
    {
        if (userDataProvider.UserId == Guid.Empty || userDataProvider.RefreshToken is null)
            throw new ForceLogoutException();

        return await RefreshTokenAsync(userDataProvider.UserId, userDataProvider.RefreshToken, cancellationToken);
    }

    public async Task<UserTokenData> RefreshTokenAsync(Guid userId, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userAgent = userAgentProvider.GetUserAgent();

        var response = await userIdentityApiClient.RefreshTokenAsync(userId, refreshToken, userAgent, cancellationToken);

        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

        userDataProvider.UpdateRefreshToken(response.ResponseData.Token, response.ResponseData.RefreshToken);

        return response.ResponseData;
    }

    public async Task ResetPasswordWithResetCodeAsync(string email, string newPassword, string resetCode, CancellationToken cancellationToken = default)
    {
        var response = await userIdentityApiClient.ResetPasswordWithEmailCodeAsync(email, newPassword, resetCode, cancellationToken);
        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
    }
}
