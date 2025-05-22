namespace ApiClients.Abstractions.Models.Responses;
public record ConversationInfoData
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required bool IsGroup { get; set; }
    //public required string? Avatar { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
}
