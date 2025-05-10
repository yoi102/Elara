using CommunityToolkit.Mvvm.ComponentModel;
using Elara.ViewModel.Interfaces;

namespace Elara.ViewModel.Chat;

public partial class MessageModel : ObservableObject, IHasNotificationNumber
{
    public MessageModel(ParticipantModel sender)
    {
        Sender = sender;
    }

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

    [ObservableProperty]
    private bool isUnread;

    public ParticipantModel Sender { get; }

    public int? NotificationNumber => throw new NotImplementedException();
}
