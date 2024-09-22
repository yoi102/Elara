using ApiClients.Abstractions.PersonalSpaceApiClient;

namespace ApiClients.Clients;

public class PersonalSpaceApiClient : IPersonalSpaceApiClient
{
    private readonly ITokenRefreshingRestClient tokenRefreshingRestClient;

    public PersonalSpaceApiClient(ITokenRefreshingRestClient tokenRefreshingRestClient)
    {
        this.tokenRefreshingRestClient = tokenRefreshingRestClient;
    }
}
