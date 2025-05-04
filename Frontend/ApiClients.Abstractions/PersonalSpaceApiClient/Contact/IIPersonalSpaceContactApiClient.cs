using ApiClients.Abstractions.PersonalSpaceApiClient.Contact.Responses;

namespace ApiClients.Abstractions.PersonalSpaceApiClient.Contact;

public interface IIPersonalSpaceContactApiClient
{
    Task<DeleteContactResponse> DeleteContactAsync(Guid contactId, CancellationToken cancellationToken = default);

    Task<GetAllContactsResponse> GetAllContactsAsync(CancellationToken cancellationToken = default);

    Task<UpdateContactInfoResponse> UpdateContactInfoAsync(Guid contactId, CancellationToken cancellationToken = default);
}
