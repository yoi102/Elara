using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLink.Domain.Results
{
    public record class LoginResult
    {

        public required SignInResult SignInResult { get; init; }

        public required string? Token { get; init; }

        [MemberNotNullWhen(true, nameof(Token))]
        public bool IsSuccess => SignInResult.Succeeded;
    }
}