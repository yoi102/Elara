using Elara.wpf.View.Dialogs;
using MaterialDesignThemes.Wpf;
using Service.Abstractions;

namespace Elara.wpf.Services;

public class DialogService : IDialogService
{
    public void ShowProgressBarDialog(object dialogIdentifier)
    {
        //ProgressBar progressBar = new ProgressBar();
        //progressBar.Margin = new Thickness(16);
        //progressBar.HorizontalAlignment = HorizontalAlignment.Center;
        //progressBar.IsIndeterminate = true;
        //progressBar.Style = Application.Current.Resources["MaterialDesignCircularProgressBar"] as Style;
        ProgressDialog progressDialog = new ProgressDialog();
        DialogHost.Show(progressDialog, dialogIdentifier);
    }

    public void CloseDialog(object dialogIdentifier)
    {
        DialogHost.Close(dialogIdentifier);

    }

    public async Task ShowMessageDialogAsync(string message, object dialogIdentifier)
    {
        MessageDialog messageDialog = new MessageDialog(message);
        await DialogHost.Show(messageDialog, dialogIdentifier);
    }
    public void TryShowProgressBarDialog(object dialogIdentifier)
    {
        var dialogSession = DialogHost.GetDialogSession(dialogIdentifier);
        ProgressDialog progressDialog = new ProgressDialog();

        if (dialogSession is not null)
        {
            dialogSession.UpdateContent(progressDialog);
        }
        else
        {
            DialogHost.Show(progressDialog, dialogIdentifier);
        }
    }

    public async Task TryShowMessageDialogAsync(string message, object dialogIdentifier)
    {
        var dialogSession = DialogHost.GetDialogSession(dialogIdentifier);
        MessageDialog messageDialog = new MessageDialog(message);

        if (dialogSession is not null)
        {
            dialogSession.UpdateContent(messageDialog);
            //await dialogSession.UpdateContent(messageDialog);//Can not await...........
        }
        else
        {
            await DialogHost.Show(messageDialog, dialogIdentifier);
        }
    }


    public void TryCloseDialog(object dialogIdentifier)
    {
        try
        {
            DialogHost.Close(dialogIdentifier);
        }
        catch (InvalidOperationException)
        {

        }
    }



}