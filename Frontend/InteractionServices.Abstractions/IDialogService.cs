namespace InteractionServices.Abstractions;

public interface IDialogService
{
    Task ShowMessageDialogAsync(string message, object dialogIdentifier);

    IDisposable ShowProgressBarDialog(object dialogIdentifier);

    void Close(object dialogIdentifier);
}
