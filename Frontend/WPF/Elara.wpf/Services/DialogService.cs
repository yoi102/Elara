using Elara.wpf.View.Dialogs;
using Frontend.Shared;
using InteractionServices.Abstractions;
using MaterialDesignThemes.Wpf;

namespace Elara.wpf.Services;

public class DialogService : IDialogService
{
    public IDisposable ShowProgressBarDialog(object dialogIdentifier)
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

        return new DeferredScope(() => { Close(dialogIdentifier); });
    }

    public void Close(object dialogIdentifier)
    {
        var dialogSession = DialogHost.GetDialogSession(dialogIdentifier);
        dialogSession?.Close();
    }

    public async Task ShowMessageDialogAsync(string message, object dialogIdentifier)
    {
        var dialogSession = DialogHost.GetDialogSession(dialogIdentifier);
        MessageDialog messageDialog = new MessageDialog(message);

        if (dialogSession is not null)
        {
            //await dialogSession.UpdateContent(messageDialog);//await 不了！！！！遗憾
            dialogSession.Close();//无奈之举，只能关闭后重开
        }
        //else
        {
            await DialogHost.Show(messageDialog, dialogIdentifier);
        }
    }
}
