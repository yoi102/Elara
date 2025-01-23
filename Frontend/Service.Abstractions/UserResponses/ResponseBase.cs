using System.Diagnostics.CodeAnalysis;

namespace Service.Abstractions.UserResponses;

public abstract record ResponseBase
{
    [MemberNotNullWhen(false, nameof(ErrorMessage))]
    public required bool IsSuccessful { get; set; }
    public string? ErrorMessage { get; set; }
}
