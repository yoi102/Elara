using ApiClients.Abstractions.PersonalSpaceApiClient.ContactRequest.Responses;

namespace ApiClients.Abstractions.PersonalSpaceApiClient.ContactRequest;

public interface IPersonalSpaceContactRequestApiClient
{
    Task<ApiResponse> AcceptContactRequestAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ContactRequestsResponse> GetReceivedContactRequestsAsync(CancellationToken cancellationToken = default);

    Task<ApiResponse> RejectContactRequestAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiResponse> SendContactRequestAsync(Guid id, CancellationToken cancellationToken = default);
}
