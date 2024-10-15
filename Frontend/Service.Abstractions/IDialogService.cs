namespace Service.Abstractions
{
    public interface IDialogService
    {
        Task ShowMessageDialogAsync(string message, object dialogIdentifier);
        void ShowProgressBarDialog(object dialogIdentifier);
        void CloseDialog(object dialogIdentifier);


        Task TryShowMessageDialogAsync(string message, object dialogIdentifier);
        void TryShowProgressBarDialog(object dialogIdentifier);
        void TryCloseDialog(object dialogIdentifier);
    }

}

