using ApiClients.Abstractions;
using ApiClients.Abstractions.Models;
using ApiClients.Abstractions.Models.Responses;
using Frontend.Shared.Exceptions;
using RestSharp;

namespace ApiClients.Clients;

internal class UserIdentityApiClient : IUserIdentityApiClient
{
    private const string serviceUri = "/IdentityService/api/users";
    private readonly IRestClient restClient;

    public UserIdentityApiClient(IRestClient restClient)
    {
        this.restClient = restClient;
    }

    public async Task<ApiResponse<UserTokenData>> RefreshTokenAsync(Guid userId, string refreshToken, string userAgent, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest
        {
            Resource = serviceUri + "/refresh-token"
        };

        restRequest.AddBody(new { UserId = userId, RefreshToken = refreshToken, UserAgent = userAgent });

        var restResponse = await restClient.PostAsync(restRequest, cancellationToken);

        if (!restResponse.IsSuccessful)
            return new ApiResponse<UserTokenData>()
            {
                StatusCode = restResponse.StatusCode,
                ErrorMessage = restResponse.ErrorMessage,
            };

        if (string.IsNullOrEmpty(restResponse.Content))
            throw new ApiResponseException();

        var responseData = JsonUtils.DeserializeInsensitive<UserTokenData>(restResponse.Content);
        if (responseData is null)
            throw new ApiResponseException();

        restClient.AddDefaultHeader("Authorization", "Bearer " + responseData.Token);

        return new ApiResponse<UserTokenData>()
        {
            IsSuccessful = true,
            StatusCode = restResponse.StatusCode,
            ResponseData = responseData
        };
    }

    public async Task<ApiResponse<ResetCodeData>> GetResetCodeByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest
        {
            Resource = serviceUri + "/get-reset-code-by-email"
        };
        restRequest.AddParameter("email", email);

        var restResponse = await restClient.GetAsync(restRequest, cancellationToken);

        if (!restResponse.IsSuccessful)
            return new ApiResponse<ResetCodeData>() { IsSuccessful = false, StatusCode = restResponse.StatusCode };

        if (string.IsNullOrEmpty(restResponse.Content))
            throw new ApiResponseException();

        var responseData = JsonUtils.DeserializeInsensitive<ResetCodeData>(restResponse.Content);
        if (responseData is null)
            throw new ApiResponseException();

        return new ApiResponse<ResetCodeData>() { IsSuccessful = true, StatusCode = restResponse.StatusCode, ErrorMessage = restResponse.ErrorMessage, ResponseData = responseData };
    }

    public async Task<ApiResponse<UserTokenData>> LoginByEmailAndPasswordAsync(string email, string password, string userAgent, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest
        {
            Resource = serviceUri + "/login-by-email-and-password"
        };

        var body = new { Email = email, Password = password, UserAgent = userAgent };
        restRequest.AddBody(body);

        var restResponse = await restClient.PostAsync(restRequest, cancellationToken);

        if (!restResponse.IsSuccessful)
            return new ApiResponse<UserTokenData>()
            {
                StatusCode = restResponse.StatusCode,
                ErrorMessage = restResponse.ErrorMessage,
            };

        if (string.IsNullOrEmpty(restResponse.Content))
            throw new ApiResponseException();

        var responseData = JsonUtils.DeserializeInsensitive<UserTokenData>(restResponse.Content);

        if (responseData is null)
            throw new ApiResponseException();

        restClient.DefaultParameters.ReplaceParameter(new HeaderParameter("Authorization", "Bearer " + responseData.Token));

        return new ApiResponse<UserTokenData>()
        {
            IsSuccessful = true,
            StatusCode = restResponse.StatusCode,
            ResponseData = responseData
        };
    }

    public async Task<ApiResponse<UserTokenData>> LoginByNameAndPasswordAsync(string name, string password, string userAgent, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest
        {
            Resource = serviceUri + "/login-by-name-and-password"
        };
        var body = new { Name = name, Password = password, UserAgent = userAgent };
        restRequest.AddBody(body);

        var restResponse = await restClient.PostAsync(restRequest, cancellationToken);

        if (!restResponse.IsSuccessful)
            return new ApiResponse<UserTokenData>()
            {
                StatusCode = restResponse.StatusCode,
                ErrorMessage = restResponse.ErrorMessage,
            };

        if (string.IsNullOrEmpty(restResponse.Content))
            throw new ApiResponseException();

        var responseData = JsonUtils.DeserializeInsensitive<UserTokenData>(restResponse.Content);

        if (responseData is null)
            throw new ApiResponseException();

        restClient.DefaultParameters.ReplaceParameter(new HeaderParameter("Authorization", "Bearer " + responseData.Token));

        return new ApiResponse<UserTokenData>()
        {
            IsSuccessful = true,
            StatusCode = restResponse.StatusCode,
            ResponseData = responseData
        };
    }

    public async Task<ApiResponse> ResetPasswordWithEmailCodeAsync(string email, string newPassword, string resetCode, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest
        {
            Resource = serviceUri + "/reset-password-with-email-code"
        };
        var body = new { Email = email, NewPassword = newPassword, ResetCode = resetCode };
        restRequest.AddBody(body);

        var restResponse = await restClient.PutAsync(restRequest, cancellationToken);

        if (!restResponse.IsSuccessful)
            return new ApiResponse { IsSuccessful = false, StatusCode = restResponse.StatusCode, ErrorMessage = restResponse.ErrorMessage };

        return new ApiResponse { IsSuccessful = true, StatusCode = restResponse.StatusCode };
    }

    public async Task<ApiResponse> SignUpAsync(string name, string email, string password, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest
        {
            Resource = serviceUri + "/sign-up"
        };
        var body = new { Name = name, Email = email, Password = password };
        restRequest.AddBody(body);

        var restResponse = await restClient.PostAsync(restRequest, cancellationToken);

        if (!restResponse.IsSuccessful)
            return new ApiResponse { IsSuccessful = false, StatusCode = restResponse.StatusCode, ErrorMessage = restResponse.ErrorMessage };

        return new ApiResponse { IsSuccessful = true, StatusCode = restResponse.StatusCode };
    }
}
