using ApiClients.Abstractions.Models.Responses;

namespace Services.Abstractions.PersonalSpaceServices;

public interface IPersonalSpaceContactService
{
    Task<ApiServiceResult> DeleteContactAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<ContactData[]>> GetContactsAsync(CancellationToken cancellationToken = default);

    Task<ApiServiceResult> UpdateContactInfoAsync(Guid id, CancellationToken cancellationToken = default);
}
