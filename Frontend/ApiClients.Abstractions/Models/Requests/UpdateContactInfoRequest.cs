namespace ApiClients.Abstractions.Models.Requests;
public record UpdateContactInfoRequest
{
    public required string Remark { get; init; }

}
