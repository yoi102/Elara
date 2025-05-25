using ApiClients.Abstractions;
using ApiClients.Abstractions.Models.Responses;
using Services.Abstractions.PersonalSpaceServices;

namespace Services.PersonalSpaceServices;

internal class PersonalSpaceContactRequestService : IPersonalSpaceContactRequestService
{
    private readonly IPersonalSpaceContactRequestApiClient personalSpaceContactRequestApiClient;

    public PersonalSpaceContactRequestService(IPersonalSpaceContactRequestApiClient personalSpaceContactRequestApiClient)
    {
        this.personalSpaceContactRequestApiClient = personalSpaceContactRequestApiClient;
    }

    public async Task AcceptContactRequestAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await personalSpaceContactRequestApiClient.AcceptContactRequestAsync(id, cancellationToken);

        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
    }

    public async Task<ContactRequestData?> GetContactRequestByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await personalSpaceContactRequestApiClient.GetContactRequestByIdAsync(id, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            return response.ResponseData;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<ContactRequestData[]> GetReceivedContactRequestsAsync(CancellationToken cancellationToken = default)
    {
        var response = await personalSpaceContactRequestApiClient.GetReceivedContactRequestsAsync(cancellationToken);

        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
        return response.ResponseData;
    }

    public async Task RejectContactRequestAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await personalSpaceContactRequestApiClient.RejectContactRequestAsync(id, cancellationToken);

        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
    }

    public async Task SendContactRequestAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await personalSpaceContactRequestApiClient.SendContactRequestAsync(id, cancellationToken);

        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
    }
}
