using ApiClients.Abstractions.Models;
using ApiClients.Abstractions.Models.Responses;

namespace ApiClients.Abstractions;

public interface IPersonalSpaceContactRequestApiClient
{
    Task<ApiResponse> AcceptContactRequestAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiResponse<ContactRequestData>> GetContactRequestByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiResponse<ContactRequestData[]>> GetReceivedContactRequestsAsync(CancellationToken cancellationToken = default);

    Task<ApiResponse> RejectContactRequestAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiResponse> SendContactRequestAsync(Guid id, CancellationToken cancellationToken = default);
}
