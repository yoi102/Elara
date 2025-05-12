using CommunityToolkit.Mvvm.ComponentModel;
using Elara.ViewModel.Interfaces;
using Services.Abstractions.Results.Data;
using System.Collections.ObjectModel;

namespace Elara.ViewModel.Chat;

public partial class MessageModel : ObservableObject, IHasNotificationNumber
{
    [ObservableProperty]
    private ObservableCollection<UploadedItemData> attachments = [];

    [ObservableProperty]
    private string content = string.Empty;

    [ObservableProperty]
    private bool isUnread;

    [ObservableProperty]
    private QuoteMessageData? quoteMessage;

    [ObservableProperty]
    private ObservableCollection<ReplyMessageModel> replyMessages = [];

    [ObservableProperty]
    private DateTimeOffset sendAt;

    [ObservableProperty]
    private DateTimeOffset? updatedAt;

    public MessageModel(MessageSenderData sender)
    {
        Sender = sender;
    }

    public int? NotificationNumber
    {
        get
        {
            var count = ReplyMessages.Count(x => x.IsUnread);
            if (count == 0) return null;
            return count;
        }
    }

    public MessageSenderData Sender { get; }
}
