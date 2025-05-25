using ExceptionHandling;
using Frontend.Shared.Identifiers;
using InteractionServices.Abstractions;

namespace Elara.ViewModel.ExceptionHandlers;

[HandlesException(typeof(HttpRequestException))]
internal class HttpRequestExceptionHandler : IExceptionHandler
{
    private readonly IDialogService dialogService;

    public HttpRequestExceptionHandler(IDialogService dialogService)
    {
        this.dialogService = dialogService;
    }


    public async Task<bool> HandleExceptionAsync(Exception exception)
    {
        if (exception is HttpRequestException requestException)
        {


            await dialogService.ShowMessageDialogAsync(requestException.Message, DialogHostIdentifiers.LoginRootDialog);

            return true; // 表示已处理
        }

        return false; // 未处理
    }
}
