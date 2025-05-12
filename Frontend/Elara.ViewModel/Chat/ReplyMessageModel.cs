using CommunityToolkit.Mvvm.ComponentModel;
using Services.Abstractions.Results.Data;
using System.Collections.ObjectModel;

namespace Elara.ViewModel.Chat;

public partial class ReplyMessageModel : ObservableObject
{
    [ObservableProperty]
    private bool isUnread;

    [ObservableProperty]
    public ObservableCollection<UploadedItemData> uploadedItems = [];

    [ObservableProperty]
    private string content = string.Empty;

    [ObservableProperty]
    private DateTimeOffset? updatedAt;

    public ReplyMessageModel(MessageSenderData sender)
    {
        Sender = sender;
    }

    public Guid MessageId { get; init; }
    public QuoteMessageData? QuoteMessage { get; init; }
    public MessageSenderData Sender { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
}
