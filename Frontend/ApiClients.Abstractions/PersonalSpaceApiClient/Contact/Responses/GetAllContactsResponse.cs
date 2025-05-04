namespace ApiClients.Abstractions.PersonalSpaceApiClient.Contact.Responses;

public record GetAllContactsResponse : ApiResponse<ContactData[]>;

public record ContactData(Guid Id, Guid ContactId, string Remark);
