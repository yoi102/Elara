namespace ApiClients.Abstractions.Models.Responses;
public record AccountInfoData
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string? Email { get; set; }
    public required string? PhoneNumber { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
}
