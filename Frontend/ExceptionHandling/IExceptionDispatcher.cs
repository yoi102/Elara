namespace ExceptionHandling;
public interface IExceptionDispatcher
{
    IExceptionHandler? GetExceptionHandler(Exception exception);
}
