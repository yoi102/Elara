using ApiClients.Abstractions.Models.Responses;

namespace Services.Abstractions.PersonalSpaceServices;

public interface IPersonalSpaceContactRequestService
{
    Task AcceptContactRequestAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ContactRequestData?> GetContactRequestByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ContactRequestData[]> GetReceivedContactRequestsAsync(CancellationToken cancellationToken = default);

    Task RejectContactRequestAsync(Guid id, CancellationToken cancellationToken = default);

    Task SendContactRequestAsync(Guid id, CancellationToken cancellationToken = default);
}
