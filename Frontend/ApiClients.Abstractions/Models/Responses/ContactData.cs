namespace ApiClients.Abstractions.Models.Responses;

public record ContactData
{
    public required Guid Id { get; set; }
    public required UserInfoData ContactUserInfo { get; set; }
    public required string Remark { get; set; }
    public required DateTimeOffset ContactedAt { get; set; }
}


