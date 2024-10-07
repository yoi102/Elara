using RestSharp;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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
            try
            {
                var restResponse = await client.GetAsync(restRequest, cancellationToken);
                if (restResponse.IsSuccessful)
                {
                    return true;
                }
            }
            catch (HttpRequestException ex)
            {
               //Do something;

            }

          
            return false;

        }








    }
}
