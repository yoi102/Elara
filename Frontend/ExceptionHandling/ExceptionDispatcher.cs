using System.Collections.ObjectModel;

namespace ExceptionHandling;

internal class ExceptionDispatcher : IExceptionDispatcher
{
    private readonly IServiceProvider _serviceProvider;
    private static ReadOnlyDictionary<Type, Type> _exceptionHandlerMap = null!;

    public ExceptionDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    internal static void SetExceptionHandlerMap(ReadOnlyDictionary<Type, Type> exceptionHandlerMap)
    {
        _exceptionHandlerMap = exceptionHandlerMap ??
            throw new ArgumentNullException(nameof(exceptionHandlerMap), "Exception handler map cannot be null.");
    }

    public async Task<bool?> DispatchAsync(Exception exception)
    {
        var exType = exception.GetType();

        // 支持多态匹配（Optional）
        var handlerType = _exceptionHandlerMap
            .FirstOrDefault(kv => kv.Key.IsAssignableFrom(exType)).Value;

        //则尝试匹配对应的处理器类型
        if (handlerType != null)
        {
            var handler = (IExceptionHandler)_serviceProvider.GetService(handlerType)!;
            return await handler.HandleExceptionAsync(exception);
        }

        //可以不做下面的，没有找到对应的处理器类型就不处理异常

        //如果没有找到对应的处理器类型，则抛出异常
        //else
        //{
        //    throw new InvalidOperationException($"No handler found for exception type: {exType.FullName}");
        //}
        return null;
    }
}
