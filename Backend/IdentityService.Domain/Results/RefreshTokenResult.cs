﻿namespace IdentityService.Domain.Results;
public record RefreshTokenResult(string? UserName,string? Token, string? RefreshToken, RefreshTokenStatus Status);
public enum RefreshTokenStatus
{
    Success,
    InvalidToken,
    UserNotFound,
    TokenMismatch,
    //TwoFactorRequired,
    //NotAllowed,
}
