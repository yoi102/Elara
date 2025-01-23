using RestSharp;
using Service.Abstractions;
using Service.Abstractions.UserResponses;
using System.Text.Json;

namespace HttpServices.Services;

public class UserService : IUserService
{
    private const string serviceUri = "SocialLink/api/user";
    private readonly RestClient client;

    public UserService(RestClient restClient)
    {
        this.client = restClient;
    }

    public async Task<DeleteUserResponse> DeleteAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var restResponse = await client.DeleteAsync(serviceUri, cancellationToken);

            if (restResponse.IsSuccessful)
            {
                return new DeleteUserResponse() { IsSuccessful = true };
            }
            return new DeleteUserResponse { IsSuccessful = false, ErrorMessage = restResponse.ErrorMessage };
        }
        catch (HttpRequestException ex)//由于懒，就先这样吧
        {

            return new DeleteUserResponse { IsSuccessful = false, ErrorMessage = ex.Message };
        }
    }

    public async Task<GetUserInfoResponse> GetUserInfoAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var restRequest = new RestRequest
            {
                Resource = serviceUri
            };
            var restResponse = await client.GetAsync(restRequest, cancellationToken);

            if (restResponse.IsSuccessful)
            {
                var userInfo = JsonSerializer.Deserialize<UserInfo>(restResponse.Content!);

                return new GetUserInfoResponse() { IsSuccessful = true, UserInfo = userInfo };
            }
            return new GetUserInfoResponse() { IsSuccessful = false };
        }
        catch (HttpRequestException ex)//由于懒，就先这样吧
        {
            return new GetUserInfoResponse { IsSuccessful = false, ErrorMessage = ex.Message };
        }
    }

    public async Task<GetEmailResetCodeResponse> GetEmailResetCodeAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            var restRequest = new RestRequest
            {
                Resource = serviceUri + "/get-email-reset-code"
            };
            restRequest.AddParameter("email", email);

            var restResponse = await client.GetAsync(restRequest, cancellationToken);

            if (restResponse.IsSuccessful)
            {
                return new GetEmailResetCodeResponse() { IsSuccessful = true };
            }

            return new GetEmailResetCodeResponse() { IsSuccessful = false, ErrorMessage = restResponse.ErrorMessage };
        }
        catch (HttpRequestException ex)//由于懒，就先这样吧
        {
            return new GetEmailResetCodeResponse { IsSuccessful = false, ErrorMessage = ex.Message };
        }
    }

    public async Task<LoginResponse> LoginByEmailAndPasswordAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest
        {
            Resource = serviceUri + "/login-by-email-and-password"
        };
        var body = new { Email = email, Password = password };
        restRequest.AddBody(body);
        try
        {
            var restResponse = await client.PostAsync(restRequest, cancellationToken);

            if (restResponse.IsSuccessful)
            {
                if (restResponse.Content == null)
                {
                    throw new Exception();
                }

                string token = restResponse.Content.Replace("\"", string.Empty);
                client.AddDefaultHeader("Authorization", "Bearer " + token);

                return new LoginResponse() { IsSuccessful = true };
            }
        }
        catch (HttpRequestException ex)//由于懒，就先这样吧
        {
            //if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            //{
            //    return new LoginResponse { IsSuccessful = false, ErrorMessage = ex.Message };
            //}
            //if (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
            //{
            //    return new LoginResponse { IsSuccessful = false, ErrorMessage = ex.Message };
            //}
            return new LoginResponse { IsSuccessful = false, ErrorMessage = ex.Message };
        }

        return new LoginResponse { IsSuccessful = false, ErrorMessage = "Error" };
    }

    public async Task<LoginResponse> LoginByNameAndPasswordAsync(string name, string password, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest
        {
            Resource = serviceUri + "/login-by-name-and-password"
        };
        var body = new { Name = name, Password = password };
        restRequest.AddBody(body);

        try
        {
            var restResponse = await client.PostAsync(restRequest, cancellationToken);

            if (restResponse.IsSuccessful)
            {
                if (restResponse.Content == null)
                {
                    throw new Exception();
                }

                var responseData = JsonSerializer.Deserialize<dynamic>(restResponse.Content);
                if (responseData == null)
                {
                    throw new Exception();
                }
                string token = responseData.Token;
                client.AddDefaultHeader("Authorization", "Bearer " + token);
                return new LoginResponse() { IsSuccessful = true };
            }
        }
        catch (HttpRequestException ex)
        {
            //if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            //{
            //    return new LoginResponse { IsSuccessful = false, ErrorMessage = ex.Message };
            //}
            //if (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
            //{
            //    return new LoginResponse { IsSuccessful = false, ErrorMessage = ex.Message };
            //}
            return new LoginResponse { IsSuccessful = false, ErrorMessage = ex.Message };
        }

        return new LoginResponse { IsSuccessful = false, ErrorMessage = "Error" };
    }

    public async Task<ResetPasswordResponse> ResetPasswordWithEmailCodeAsync(string email, string newPassword, string resetCode, CancellationToken cancellationToken = default)
    {
        try
        {
            var restRequest = new RestRequest
            {
                Resource = serviceUri + "/reset-password-with-email-code"
            };
            var body = new { Email = email, NewPassword = newPassword, ResetCode = resetCode };
            restRequest.AddBody(body);

            var restResponse = await client.PostAsync(restRequest, cancellationToken);

            if (restResponse.IsSuccessful)
            {
                return new ResetPasswordResponse { IsSuccessful = true };
            }

            return new ResetPasswordResponse { IsSuccessful = true, ErrorMessage = restResponse.ErrorMessage };
        }
        catch (HttpRequestException ex)//由于懒，就先这样吧
        {
            return new ResetPasswordResponse { IsSuccessful = false, ErrorMessage = ex.Message };
        }
    }

    public async Task<CreateResponse> CreateAsync(string name, string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            var restRequest = new RestRequest
            {
                Resource = serviceUri + "/sign-up"
            };
            var body = new { Name = name, Email = email, Password = password };
            restRequest.AddBody(body);

            var restResponse = await client.PostAsync(restRequest, cancellationToken);

            if (restResponse.IsSuccessful)
            {
                return new CreateResponse { IsSuccessful = true };
            }

            return new CreateResponse { IsSuccessful = false, ErrorMessage = restResponse.ErrorMessage };
        }
        catch (HttpRequestException ex)//由于懒，就先这样吧
        {
            return new CreateResponse { IsSuccessful = false, ErrorMessage = ex.Message };
        }
    }
}