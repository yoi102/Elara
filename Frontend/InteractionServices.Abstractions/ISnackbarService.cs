namespace InteractionServices.Abstractions;

public interface ISnackbarService
{
    void EnqueueInAll(object content, TimeSpan? durationOverride = null, bool promote = false, bool neverConsiderToBeDuplicate = false);
    void Enqueue(object identifier, object content, TimeSpan? durationOverride = null, bool promote = false, bool neverConsiderToBeDuplicate = false);
}
