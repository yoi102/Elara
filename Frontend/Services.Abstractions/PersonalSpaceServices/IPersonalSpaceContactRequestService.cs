using ApiClients.Abstractions.PersonalSpaceApiClient.ContactRequest.Responses;
using Services.Abstractions.Results;

namespace Services.Abstractions.PersonalSpaceServices;

public interface IPersonalSpaceContactRequestService
{
    Task<ApiServiceResult> AcceptContactRequestAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<ContactRequestData[]>> GetReceivedContactRequestsAsync(CancellationToken cancellationToken = default);

    Task<ApiServiceResult> RejectContactRequestAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiServiceResult> SendContactRequestAsync(Guid id, CancellationToken cancellationToken = default);
}
