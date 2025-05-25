namespace ExceptionHandling;

public interface IExceptionHandler
{
    Task HandleExceptionAsync(Exception exception);
}
