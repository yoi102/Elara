using ApiClients.Abstractions.FileApiClient.Responses;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Elara.ViewModel.Interfaces;
using Services.Abstractions.ChatServices;
using Services.Abstractions.Results.Data;
using System.Collections.ObjectModel;

namespace Elara.ViewModel.Chat;

public partial class MessageViewModel : ObservableObject, IHasNotificationNumber
{
    private readonly IChatMessageService chatMessageService;

    [ObservableProperty]
    private ObservableCollection<FileItemData> attachments = [];

    [ObservableProperty]
    private MessageData? messageData;

    [ObservableProperty]
    private ObservableCollection<ReplyMessageViewModel> replyMessages = [];

    public MessageViewModel(IChatMessageService chatMessageService)
    {
        this.chatMessageService = chatMessageService;
    }

    public int? NotificationNumber
    {
        get
        {
            var count = ReplyMessages.Count(x => x.ReplyMessageData!.IsUnread);
            if (count == 0) return null;
            return count;
        }
    }

    public UserInfoData? Sender { get; set; }

    [RelayCommand]
    public void RelyMessage()
    {
        //await  chatMessageService.ReplyMessageAsync();
    }
}
