using ApiClients.Abstractions.PersonalSpaceApiClient.ContactRequest.Responses;

namespace ApiClients.Abstractions.PersonalSpaceApiClient.ContactRequest;

public interface IPersonalSpaceContactRequestApiClient
{
    Task<AcceptContactRequestResponse> AcceptContactRequestAsync(Guid id, CancellationToken cancellationToken = default);

    Task<GetContactRequestsResponse> GetContactRequestsAsync(CancellationToken cancellationToken = default);

    Task<RejectContactRequestResponse> RejectContactRequestAsync(Guid id, CancellationToken cancellationToken = default);

    Task<SendContactRequestResponse> SendContactRequestAsync(Guid id, CancellationToken cancellationToken = default);
}
