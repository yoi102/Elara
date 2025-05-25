namespace InteractionServices.Abstractions;

public interface IDialogService
{
    void Close(object dialogIdentifier);

    Task ShowOrReplaceMessageDialogAsync(string message, object dialogIdentifier);

    Task ShowOrReplaceMessageInActiveWindowAsync(string message);

    IDisposable ShowProgressBarDialog(object dialogIdentifier);
}
