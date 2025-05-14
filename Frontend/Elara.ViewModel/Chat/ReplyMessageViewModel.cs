using ApiClients.Abstractions.FileApiClient.Responses;
using CommunityToolkit.Mvvm.ComponentModel;
using Services.Abstractions.Results.Data;
using System.Collections.ObjectModel;

namespace Elara.ViewModel.Chat;

public partial class ReplyMessageViewModel : ObservableObject
{
    [ObservableProperty]
    public ObservableCollection<FileItemData> uploadedItems = [];

    [ObservableProperty]
    private ReplyMessageData? replyMessageData;
}
