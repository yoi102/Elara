using System.Diagnostics.CodeAnalysis;

namespace Services.Abstractions.Results;
public record ApiServiceResult
{
    [MemberNotNullWhen(false, nameof(ErrorMessage))]
    public bool IsSuccessful { get; init; }
    public string? ErrorMessage { get; init; }
    public bool IsServerError { get; init; }
}
public record ApiServiceResult<T> where T : class
{
    [MemberNotNullWhen(true, nameof(ResultData))]
    [MemberNotNullWhen(false, nameof(ErrorMessage))]
    public bool IsSuccessful { get; init; }
    public string? ErrorMessage { get; init; }
    public bool IsServerError { get; init; }
    public T? ResultData { get; init; }
}
