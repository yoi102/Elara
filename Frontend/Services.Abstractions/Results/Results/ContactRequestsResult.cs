namespace Services.Abstractions.Results.Results;

public enum RequestStatus
{
    Pending,
    Accepted,
    Rejected
}

public record ContactRequestsResult : ApiServiceResult<ContactRequestResultData[]>;

public record ContactRequestResultData(Guid Id, Guid SenderId, Guid ReceiverId, RequestStatus Status, DateTimeOffset CreatedAt, DateTimeOffset? UpdatedAt);
