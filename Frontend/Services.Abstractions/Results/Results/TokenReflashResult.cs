namespace Services.Abstractions.Results.Results;

public record TokenReflashResult : ApiServiceResult<UserTokenResultData>;

public record UserTokenResultData(Guid UserId, string UserName, string AccessToken, string RefreshToken);
