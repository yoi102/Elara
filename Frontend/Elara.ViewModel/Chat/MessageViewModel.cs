using CommunityToolkit.Mvvm.ComponentModel;

namespace Elara.ViewModel.Chat;

public partial class MessageModel : ObservableValidator
{
    [ObservableProperty]
    private MessageAttachmentModel[]? attachments;

    [ObservableProperty]
    private string content = string.Empty;

    [ObservableProperty]
    private DateTimeOffset sendAt;

    [ObservableProperty]
    private string? senderName;

    [ObservableProperty]
    private DateTimeOffset? updateAt;
    public required ParticipantModel Sender { get; init; }
    public Guid SenderId { get; init; }
}
