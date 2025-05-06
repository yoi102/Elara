using Services.Abstractions.Results;
using Services.Abstractions.Results.Results;

namespace Services.Abstractions.PersonalSpaceServices;

public interface IPersonalSpaceContactRequestService
{
    Task<ApiServiceResult> AcceptContactRequestAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ContactRequestsResult> GetReceivedContactRequestsAsync(CancellationToken cancellationToken = default);

    Task<ApiServiceResult> RejectContactRequestAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiServiceResult> SendContactRequestAsync(Guid id, CancellationToken cancellationToken = default);
}
