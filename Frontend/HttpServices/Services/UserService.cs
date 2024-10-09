using RestSharp;
using Service.Abstractions;

namespace HttpServices.Services
{
    public class UserService : IUserService
    {
        private readonly RestClient client;
        private const string serviceUri = "SocialLink/api/user";
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








    }
}
