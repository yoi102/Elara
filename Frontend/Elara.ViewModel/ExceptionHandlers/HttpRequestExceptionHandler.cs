using ExceptionHandling;
using Frontend.Shared.Exceptions;
using InteractionServices.Abstractions;

namespace Elara.ViewModel.ExceptionHandlers;

[HandlesException(typeof(HttpRequestException))]
internal class HttpRequestExceptionHandler : IExceptionHandler
{
    private readonly IDialogService dialogService;
    private readonly ISnackbarService snackbarService;

    public HttpRequestExceptionHandler(IDialogService dialogService, ISnackbarService snackbarService)
    {
        this.dialogService = dialogService;
        this.snackbarService = snackbarService;
    }

    public async Task<bool> HandleExceptionAsync(Exception exception)
    {
        if (exception is not HttpRequestException requestException)
            return false;

        if (requestException.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            await dialogService.ShowOrReplaceMessageInActiveWindowAsync("登录已过期");
            throw new ForceLogoutException();
        }

        //if (requestException.StatusCode is null || (int)requestException.StatusCode >= 500)
        //{
        //    // 服务器错误或未知状态码，
        //}

        snackbarService.EnqueueInAll($"Message: {requestException.Message}", TimeSpan.FromSeconds(2));
        var message = $"Message: \r\n{requestException.Message}";
        await dialogService.ShowOrReplaceMessageInActiveWindowAsync(message);
        return true;
    }
}
