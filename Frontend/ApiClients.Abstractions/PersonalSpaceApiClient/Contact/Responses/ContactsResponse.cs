namespace ApiClients.Abstractions.PersonalSpaceApiClient.Contact.Responses;

public record ContactsResponse : ApiResponse<ContactData[]>;

public record ContactData(Guid Id, Guid ContactId, string Remark, DateTimeOffset CreatedAt, DateTimeOffset? UpdatedAt);
