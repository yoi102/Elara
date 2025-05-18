using Frontend.Shared;

namespace ApiClients.Abstractions.PersonalSpaceApiClient.ContactRequest.Responses;

public record ContactRequestsResponse : ApiResponse<ContactRequestData[]>;
public record ContactRequestResponse : ApiResponse<ContactRequestData>;

public record ContactRequestData(Guid Id, Guid SenderId, Guid ReceiverId, RequestStatus Status, DateTimeOffset CreatedAt, DateTimeOffset? UpdatedAt);
