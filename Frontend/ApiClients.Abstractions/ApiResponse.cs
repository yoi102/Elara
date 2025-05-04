using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace ApiClients.Abstractions;

public record ApiResponse<T> where T : class
{
    [MemberNotNullWhen(false, nameof(ErrorMessage))]
    [MemberNotNullWhen(true, nameof(ResponseData))]
    public bool IsSuccessful { get; init; }
    public string? ErrorMessage { get; init; }
    public required HttpStatusCode StatusCode { get; init; }
    public DateTime Timestamp { get; } = DateTime.UtcNow;
    //public Guid RequestId { get; } = Guid.NewGuid();
    [DisallowNull]
    public T? ResponseData { get; init; }
}

public record ApiResponse
{
    [MemberNotNullWhen(false, nameof(ErrorMessage))]
    public bool IsSuccessful { get; init; }
    public string? ErrorMessage { get; init; }
    public required HttpStatusCode StatusCode { get; init; }
    public DateTime Timestamp { get; } = DateTime.UtcNow;
    //public Guid RequestId { get; } = Guid.NewGuid();
}
