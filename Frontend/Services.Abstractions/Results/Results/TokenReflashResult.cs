namespace Services.Abstractions.Results.Results;

public record TokenReflashResult : ApiServiceResult<TokenReflashResultData>;

public record TokenReflashResultData(string AccessToken, string RefreshToken);
