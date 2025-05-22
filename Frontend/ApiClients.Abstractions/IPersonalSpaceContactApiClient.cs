using ApiClients.Abstractions.Models;
using ApiClients.Abstractions.Models.Responses;

namespace ApiClients.Abstractions;

public interface IPersonalSpaceContactApiClient
{
    Task<ApiResponse> DeleteContactAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiResponse<ContactData[]>> GetContactsAsync(CancellationToken cancellationToken = default);

    Task<ApiResponse> UpdateContactInfoAsync(Guid id, CancellationToken cancellationToken = default);
}
