namespace IdentityService.Domain.Results;
public record RefreshTokenResult(string? UserName,string? Token, string? ReflashToken, RefreshTokenStatus Status);
public enum RefreshTokenStatus
{
    Success,
    InvalidToken,
    UserNotFound,
    TokenMismatch,
    //TwoFactorRequired,
    //NotAllowed,
}
