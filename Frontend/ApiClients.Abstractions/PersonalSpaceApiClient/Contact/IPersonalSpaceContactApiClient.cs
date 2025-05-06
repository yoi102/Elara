using ApiClients.Abstractions.PersonalSpaceApiClient.Contact.Responses;

namespace ApiClients.Abstractions.PersonalSpaceApiClient.Contact;

public interface IPersonalSpaceContactApiClient
{
    Task<ApiResponse> DeleteContactAsync(Guid contactId, CancellationToken cancellationToken = default);

    Task<GetAllContactsResponse> GetContactsAsync(CancellationToken cancellationToken = default);

    Task<ApiResponse> UpdateContactInfoAsync(Guid contactId, CancellationToken cancellationToken = default);
}
