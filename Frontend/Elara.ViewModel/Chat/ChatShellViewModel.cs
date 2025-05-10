using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Elara.ViewModel.Interfaces;
using System.Collections.ObjectModel;

namespace Elara.ViewModel.Chat;

public partial class ChatShellViewModel : ObservableObject, IHasNotificationNumber
{
    [ObservableProperty]
    private ObservableCollection<ConversationModel> conversations = [];

    [ObservableProperty]
    private ConversationModel? selectedConversation;

    [ObservableProperty]
    private ChatViewModel? chatViewModel;

    public int? NotificationNumber
    {
        get
        {
            var unreadCount = Conversations.SelectMany(c => c.Messages)
                                              .Count(m => m.IsUnread);
            if (unreadCount == 0)
                return null;

            return unreadCount;
        }
    }

    [RelayCommand]
    private void CreateGroupConversation()
    {
    }
}
