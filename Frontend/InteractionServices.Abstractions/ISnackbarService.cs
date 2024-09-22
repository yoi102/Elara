namespace InteractionServices.Abstractions;

public interface ISnackbarService
{
    void Enqueue(object identifier, object content, TimeSpan? durationOverride = null, bool promote = false, bool neverConsiderToBeDuplicate = false);
}
