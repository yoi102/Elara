using CommunityToolkit.Mvvm.ComponentModel;
using Elara.ViewModel.Interfaces;
using System.Collections.ObjectModel;

namespace Elara.ViewModel.Chat;

public partial class ConversationModel : ObservableObject, IHasNotificationNumber
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(LatestMessage))]
    private ObservableCollection<MessageModel> messages = [];

    [ObservableProperty]
    private string name = "";

    [NotifyPropertyChangedFor(nameof(NotificationNumber))]
    [ObservableProperty]
    private ObservableCollection<ParticipantModel> participants = [];

    public Guid Id { get; init; }
    public bool IsGroup { get; init; }
    public MessageModel? LatestMessage => Messages?.FirstOrDefault();

    public DateTimeOffset CreateAt { get; init; }

    public int? NotificationNumber
    {
        get
        {
            var unreadCount = Messages.Count(m => m.IsUnread);
            if (unreadCount == 0)
                return null;

            return unreadCount;
        }
    }
}
