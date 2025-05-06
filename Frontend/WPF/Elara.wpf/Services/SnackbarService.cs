using Elara.wpf.Controls.Assists;
using InteractionServices.Abstractions;

namespace Elara.wpf.Services;

public class SnackbarService : ISnackbarService
{
    public void Enqueue(object identifier, object content, TimeSpan? durationOverride = null, bool promote = false, bool neverConsiderToBeDuplicate = false)
    {
        if (!SnackbarIdentifierAssist.SnackbarGroups.TryGetValue(identifier, out var snackbars))
            return;

        foreach (var snackbar in snackbars)
        {
            snackbar.MessageQueue?.Enqueue(
                content,
                null,
                null,
                null,
                promote,
                neverConsiderToBeDuplicate,
                durationOverride);
        }
    }

    public void Enqueue(object identifier, object content)
    {
        if (!SnackbarIdentifierAssist.SnackbarGroups.TryGetValue(identifier, out var snackbars))
            return;
        foreach (var snackbar in snackbars)
            snackbar.MessageQueue?.Enqueue(content);
    }

    public void Enqueue(object identifier, object content, bool neverConsiderToBeDuplicate)
    {
        if (!SnackbarIdentifierAssist.SnackbarGroups.TryGetValue(identifier, out var snackbars))
            return;
        foreach (var snackbar in snackbars)
            snackbar.MessageQueue?.Enqueue(content, neverConsiderToBeDuplicate);
    }

    public void Enqueue(object identifier, object content, object? actionContent, Action? actionHandler)
    {
        if (!SnackbarIdentifierAssist.SnackbarGroups.TryGetValue(identifier, out var snackbars))
            return;
        foreach (var snackbar in snackbars)
            snackbar.MessageQueue?.Enqueue(content, actionContent, actionHandler);
    }

    public void Enqueue<TArgument>(object identifier, object content, object? actionContent, Action<TArgument?>? actionHandler, TArgument? actionArgument)
    {
        if (!SnackbarIdentifierAssist.SnackbarGroups.TryGetValue(identifier, out var snackbars))
            return;
        foreach (var snackbar in snackbars)
            snackbar.MessageQueue?.Enqueue(content, actionContent, actionHandler, actionArgument);
    }

    public void Enqueue<TArgument>(object identifier, object content, object? actionContent, Action<TArgument?>? actionHandler, TArgument? actionArgument, bool promote)
    {
        if (!SnackbarIdentifierAssist.SnackbarGroups.TryGetValue(identifier, out var snackbars))
            return;
        foreach (var snackbar in snackbars)
            snackbar.MessageQueue?.Enqueue(content, actionContent, actionHandler, actionArgument, promote);
    }

    public void Enqueue<TArgument>(object identifier, object content, object? actionContent, Action<TArgument?>? actionHandler, TArgument? actionArgument, bool promote, bool neverConsiderToBeDuplicate, TimeSpan? durationOverride = null)
    {
        if (!SnackbarIdentifierAssist.SnackbarGroups.TryGetValue(identifier, out var snackbars))
            return;
        foreach (var snackbar in snackbars)
            snackbar.MessageQueue?.Enqueue(content, actionContent, actionHandler, actionArgument, promote, neverConsiderToBeDuplicate, durationOverride);
    }
}
