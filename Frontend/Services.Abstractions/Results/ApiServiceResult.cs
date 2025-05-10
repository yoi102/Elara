using System.Diagnostics.CodeAnalysis;

namespace Services.Abstractions.Results;
public record ApiServiceResult
{
    [MemberNotNullWhen(false, nameof(ErrorMessage))]
    public virtual bool IsSuccessful { get; init; }
    public string? ErrorMessage { get; init; }
    [MemberNotNullWhen(true, nameof(ErrorMessage))]
    public virtual bool IsServerError { get; init; }
}
public record ApiServiceResult<T> : ApiServiceResult where T : class
{
    public ApiServiceResult()
    {
    }

    [MemberNotNullWhen(true, nameof(ResultData))]
    public override bool IsSuccessful { get; init; }
    public override bool IsServerError { get; init; }
    public T? ResultData { get; init; }

    public static ApiServiceResult<T> FromFailure(ApiServiceResult source)
    {
        if (source.IsSuccessful)
            throw new ArgumentException("Cannot create a failure result from a successful result.", nameof(source));

        return new ApiServiceResult<T>()
        {
            ErrorMessage = source.ErrorMessage,
            IsServerError = source.IsServerError,
        };
    }
}

public record ApiServiceSimpleResult<T> : ApiServiceResult where T : class
{
    public ApiServiceSimpleResult()
    {
    }

    public T? ResultData { get; init; }

    public static ApiServiceSimpleResult<T> FromFailure(ApiServiceResult source)
    {
        if (source.IsSuccessful)
            throw new ArgumentException("Cannot create a failure result from a successful result.", nameof(source));

        return new ApiServiceSimpleResult<T>()
        {
            ErrorMessage = source.ErrorMessage,
            IsServerError = source.IsServerError,
        };
    }
}
