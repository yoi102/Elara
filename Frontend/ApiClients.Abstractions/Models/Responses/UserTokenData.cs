namespace ApiClients.Abstractions.Models.Responses;

public record UserTokenData(Guid UserId, string UserName, string Token, string RefreshToken);
