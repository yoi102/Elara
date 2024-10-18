using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstractions.UserResponses
{
    public abstract record ResponseBase
    {
        [MemberNotNullWhen(false, nameof(ErrorMessage))]
        public required bool IsSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
