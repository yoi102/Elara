using Elara.wpf.Extensions;
using Elara.wpf.View.Dialogs;
using Frontend.Shared;
using InteractionServices.Abstractions;
using MaterialDesignThemes.Wpf;
using System.Windows;

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

    public async Task ShowOrReplaceMessageDialogAsync(string header, string message, object dialogIdentifier)
    {
        var dialogSession = DialogHost.GetDialogSession(dialogIdentifier);
        if (dialogSession is not null)
        {
            //await dialogSession.UpdateContent(messageDialog);//await 不了！！！！遗憾，不能直接更新
            dialogSession.Close();//无奈之举，只能关闭后重开
        }

        MessageDialog messageDialog = new MessageDialog(header, message);
        await DialogHost.Show(messageDialog, dialogIdentifier);
    }

    public async Task ShowOrReplaceMessageInActiveWindowAsync(string header, string message)
    {
        var activeWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
        activeWindow ??= WindowTracker.LastActivatedWindow;
        activeWindow ??= Application.Current.MainWindow;
        if (activeWindow is null)
            return;

        var dialogHost = GetFirstDialogHost(activeWindow);
        if (dialogHost is null)
            return;

        // 关闭当前打开的对话框，确保新的对话框可以正确显示
        var identifier = dialogHost.Identifier;

        var dialogSession = DialogHost.GetDialogSession(identifier);
        dialogSession?.Close();

        MessageDialog messageDialog = new MessageDialog(header, message);
        await dialogHost.ShowDialog(messageDialog);
    }

    private static DialogHost? GetFirstDialogHost(Window window)
    {
        if (window is null) throw new ArgumentNullException(nameof(window));

        DialogHost? dialogHost = window.VisualDepthFirstTraversal().OfType<DialogHost>().FirstOrDefault();

        return dialogHost;
    }
}
