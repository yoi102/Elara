namespace Services.Abstractions.Results.Data;
public record ParticipantData
{
    public required UserInfoData UserInfoData { get; init; }
    public required string Role { get; init; }


}
