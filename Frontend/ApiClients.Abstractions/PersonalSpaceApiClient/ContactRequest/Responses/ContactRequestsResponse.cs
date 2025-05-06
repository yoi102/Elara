namespace ApiClients.Abstractions.PersonalSpaceApiClient.ContactRequest.Responses;

public enum RequestStatus
{
    Pending,
    Accepted,
    Rejected
}

public record ContactRequestsResponse : ApiResponse<ContactRequestData[]>;

public record ContactRequestData(Guid Id, Guid SenderId, Guid ReceiverId, RequestStatus Status, DateTimeOffset CreatedAt, DateTimeOffset? UpdatedAt);
