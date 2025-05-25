using ApiClients.Abstractions.Models.Responses;

namespace Services.Abstractions.PersonalSpaceServices;

public interface IPersonalSpaceContactService
{
    Task DeleteContactAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ContactData[]> GetContactsAsync(CancellationToken cancellationToken = default);

    Task UpdateContactInfoAsync(Guid id, CancellationToken cancellationToken = default);
}
