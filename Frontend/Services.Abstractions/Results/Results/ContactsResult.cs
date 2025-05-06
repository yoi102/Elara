namespace Services.Abstractions.Results.Results;

public record ContactsResult : ApiServiceResult<ContactsResultData[]>;

public record ContactsResultData(Guid Id, Guid ContactId, string Remark, DateTimeOffset CreatedAt, DateTimeOffset? UpdatedAt);
