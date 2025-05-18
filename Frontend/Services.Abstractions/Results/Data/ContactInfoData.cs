namespace Services.Abstractions.Results.Data;
public record ContactInfoData
{
    public required Guid Id { get; init; }
    public required string Remark { get; init; }
    public required UserInfoData UserInfo { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required DateTimeOffset? UpdatedAt { get; init; }
}
