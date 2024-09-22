namespace Services.Abstractions.Results.Results;

public record UserInfoResult : ApiServiceResult<UserInfoResultData>;

public record UserInfoResultData(Guid Id, string Name, string? Email, string? PhoneNumber, DateTimeOffset CreationTime);
