namespace Frontend.Shared;

public sealed class EmptyDisposable : IDisposable
{
    public static readonly EmptyDisposable Instance = new();

    private EmptyDisposable()
    { }

    public void Dispose()
    {
        // No-op
    }
}
