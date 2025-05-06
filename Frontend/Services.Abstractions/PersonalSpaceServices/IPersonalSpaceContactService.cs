using Services.Abstractions.Results;
using Services.Abstractions.Results.Results;

namespace Services.Abstractions.PersonalSpaceServices;

public interface IPersonalSpaceContactService
{
    Task<ApiServiceResult> DeleteContactAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ContactsResult> GetContactsAsync(CancellationToken cancellationToken = default);

    Task<ApiServiceResult> UpdateContactInfoAsync(Guid id, CancellationToken cancellationToken = default);
}
