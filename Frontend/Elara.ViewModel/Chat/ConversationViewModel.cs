using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Elara.ViewModel.Interfaces;
using Services.Abstractions.Results.Data;
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
    private ObservableCollection<ParticipantData> participants = [];

    public Guid Id { get; init; }
    public bool IsGroup { get; init; }
    public MessageModel? LatestMessage => Messages?.FirstOrDefault();

    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; init; }

    public int? NotificationNumber
    {
        get
        {
            var unreadMessageCount = Messages.Count(m => m.IsUnread);
            var unreadReplyMessageCount = Messages.Sum(x => x.NotificationNumber ?? 0);

            var unreadCount = unreadMessageCount + unreadReplyMessageCount;
            if (unreadCount == 0)
                return null;

            return unreadCount;
        }
    }

    [RelayCommand]
    private void SendMessage()
    {

    }


}
