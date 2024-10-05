using Microsoft.AspNetCore.Identity;
using SocialLink.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLink.Domain.Results
{
    public record class SignUpResult
    {

        public required IdentityResult IdentityResult { get; set; }
        public User? User { get; set; }
        [MemberNotNullWhen(true, nameof(User))]
        public bool Succeeded => IdentityResult.Succeeded;
    }
}
