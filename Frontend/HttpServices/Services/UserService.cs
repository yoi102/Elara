using RestSharp;
using Service.Abstractions;
using System.Text;
using System.Text.Json;

namespace HttpServices.Services
{
    public class UserService : IUserService
    {
        private const string serviceUri = "SocialLink/api/user";
        private readonly RestClient client;
        public UserService(RestClient restClient)
        {
            this.client = restClient;
        }

        public async Task<bool> DeleteAsync(CancellationToken cancellationToken = default)
        {
            var restResponse = await client.DeleteAsync(serviceUri, cancellationToken);

            if (restResponse.IsSuccessful)
            {
                return true;
            }
            return false;
        }
        public async Task<string?> GetUserInfoAsync(CancellationToken cancellationToken = default)
        {
            var restRequest = new RestRequest
            {
                Resource = serviceUri 
            };
            var restResponse = await client.GetAsync(restRequest, cancellationToken);

            if (restResponse.IsSuccessful)
            {
                return restResponse.Content;
            }
            return string.Empty;
        }

        public async Task<bool> GetEmailResetCodeAsync(string email, CancellationToken cancellationToken = default)
        {
            var restRequest = new RestRequest
            {
                Resource = serviceUri + "/get-email-reset-code"
            };
            restRequest.AddParameter("email", email);

            var restResponse = await client.GetAsync(restRequest, cancellationToken);

            if (restResponse.IsSuccessful)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> LoginByEmailAndPasswordAsync(string email, string password, CancellationToken cancellationToken = default)
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

                 
                    string token = restResponse.Content.Replace("\"",string.Empty);
                    client.AddDefaultHeader("Authorization", "Bearer " + token);
                
                    return true;
                }
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return false;
                }

                throw;
            }

            return false;
        }

        public async Task<bool> LoginByNameAndPasswordAsync(string name, string password, CancellationToken cancellationToken = default)
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
                    return true;
                }


            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return false;
                }

                throw;
            }

            return false;
        }

        public async Task<bool> ResetPasswordWithEmailCodeAsync(string email, string newPassword, string resetCode, CancellationToken cancellationToken = default)
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
                return true;
            }

            return false;
        }

        public async Task<bool> SignUpAsync(string name, string email, string password, CancellationToken cancellationToken = default)
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
                return true;
            }

            return false;
        }
    }
}