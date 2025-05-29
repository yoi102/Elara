using ApiClients.Abstractions;
using ApiClients.Abstractions.Models.Requests;
using ApiClients.Abstractions.Models.Responses;
using Services.Abstractions.PersonalSpaceServices;

namespace Services.PersonalSpaceServices;

internal class PersonalSpaceContactService : IPersonalSpaceContactService
{
    private readonly IPersonalSpaceContactApiClient personalSpaceContactApiClient;

    public PersonalSpaceContactService(IPersonalSpaceContactApiClient personalSpaceContactApiClient)
    {
        this.personalSpaceContactApiClient = personalSpaceContactApiClient;
    }

    public async Task DeleteContactAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await personalSpaceContactApiClient.DeleteContactAsync(id, cancellationToken);

        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
    }

    public async Task<ContactData[]> GetContactsAsync(CancellationToken cancellationToken = default)
    {
        var response = await personalSpaceContactApiClient.GetContactsAsync(cancellationToken);

        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
        return response.ResponseData;
    }

    public async Task UpdateContactInfoAsync(Guid id, UpdateContactInfoRequest updateContactInfoRequest, CancellationToken cancellationToken = default)
    {
        var response = await personalSpaceContactApiClient.UpdateContactInfoAsync(id, updateContactInfoRequest, cancellationToken);

        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
    }
}
