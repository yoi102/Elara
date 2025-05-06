using ApiClients.Abstractions.PersonalSpaceApiClient.Contact.Responses;

namespace ApiClients.Abstractions.PersonalSpaceApiClient.Contact;

public interface IPersonalSpaceContactApiClient
{
    Task<ApiResponse> DeleteContactAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ContactsResponse> GetContactsAsync(CancellationToken cancellationToken = default);

    Task<ApiResponse> UpdateContactInfoAsync(Guid id, CancellationToken cancellationToken = default);
}
