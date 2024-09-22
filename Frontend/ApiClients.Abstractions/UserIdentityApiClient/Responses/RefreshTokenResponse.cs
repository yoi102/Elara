namespace ApiClients.Abstractions.UserIdentityApiClient.Responses;
public record RefreshTokenResponse : ApiResponse<UserTokenData>;

public record UserTokenData(Guid UserId, string UserName, string Token, string RefreshToken);
