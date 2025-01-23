using IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace IdentityService.Domain.Results;

public record class SignUpResult
{
    public required IdentityResult IdentityResult { get; set; }
    public User? User { get; set; }
    [MemberNotNullWhen(true, nameof(User))]
    public bool Succeeded => IdentityResult.Succeeded;
}