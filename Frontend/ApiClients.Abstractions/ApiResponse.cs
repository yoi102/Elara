using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace ApiClients.Abstractions;

public record ApiResponse<T> : ApiResponse where T : class
{
    [MemberNotNullWhen(false, nameof(ErrorMessage))]
    [MemberNotNullWhen(true, nameof(ResponseData))]
    public override bool IsSuccessful { get; init; }
    public override string? ErrorMessage { get; init; }
    [DisallowNull]
    public T? ResponseData { get; init; }
}

public record SimpleApiResponse<T> : SimpleApiResponse where T : class
{
    public T? ResponseData { get; init; }
}

public record ApiResponse : SimpleApiResponse
{
    [MemberNotNullWhen(false, nameof(ErrorMessage))]
    public override bool IsSuccessful { get; init; }
    public override string? ErrorMessage { get; init; }
}
public record SimpleApiResponse
{
    public virtual bool IsSuccessful { get; init; }
    public virtual string? ErrorMessage { get; init; }
    public required HttpStatusCode StatusCode { get; init; }
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}
