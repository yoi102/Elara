namespace Frontend.Shared.Exceptions;

public class ForceLogoutException : Exception
{
    public ForceLogoutException()
    {
    }

    public ForceLogoutException(string message) : base(message)
    {
    }
}
