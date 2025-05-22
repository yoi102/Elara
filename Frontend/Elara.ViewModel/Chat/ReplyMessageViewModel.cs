using ApiClients.Abstractions.Models.Responses;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Elara.ViewModel.Chat;

public partial class ReplyMessageViewModel : ObservableObject
{
    [ObservableProperty]
    public ObservableCollection<UploadedItemData> uploadedItems = [];

    [ObservableProperty]
    private ReplyMessageData? replyMessageData;
}
