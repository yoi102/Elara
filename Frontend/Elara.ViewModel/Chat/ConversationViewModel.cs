using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Elara.ViewModel.Interfaces;
using Services.Abstractions.ChatServices;
using Services.Abstractions.Results.Data;
using System.Collections.ObjectModel;

namespace Elara.ViewModel.Chat;

public partial class ConversationViewModel : ObservableObject, IHasNotificationNumber
{

    public ConversationViewModel(IChatMessageService chatMessageService)
    {
        this.chatMessageService = chatMessageService;
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(LatestMessage))]
    private ObservableCollection<MessageViewModel> messages = [];

    [NotifyPropertyChangedFor(nameof(NotificationNumber))]
    [ObservableProperty]
    private ObservableCollection<ParticipantData> participants = [];
    public MessageViewModel? LatestMessage => Messages?.FirstOrDefault();


    [ObservableProperty]
    private ConversationData? conversationData;

    public int? NotificationNumber
    {
        get
        {
            var unreadMessageCount = Messages.Count(m => m.MessageData!.IsUnread);
            var unreadReplyMessageCount = Messages.Sum(x => x.NotificationNumber ?? 0);

            var unreadCount = unreadMessageCount + unreadReplyMessageCount;
            if (unreadCount == 0)
                return null;

            return unreadCount;
        }
    }


    [ObservableProperty]
    private string drafts = string.Empty;
    [ObservableProperty]
    private ObservableCollection<UploadedItemData> draftsAttachments = [];
    private readonly IChatMessageService chatMessageService;

    [RelayCommand]
    private async Task SendMessageAsync()
    {
      //await  chatMessageService.SendMessageAsync()


    }

    [RelayCommand]
    private void UploadFile()
    {



    }





}
