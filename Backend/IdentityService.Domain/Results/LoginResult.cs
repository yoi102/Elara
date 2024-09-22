using DomainCommons.EntityStronglyIds;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace IdentityService.Domain.Results;

public record class LoginResult
{
    public required SignInResult SignInResult { get; init; }

    public UserId UserId { get; init; }
    public string? UserName { get; init; }
    public string? Token { get; init; }
    public string? RefreshToken { get; init; }

    [MemberNotNullWhen(true, nameof(Token))]
    public bool IsSuccess => SignInResult.Succeeded;
}
