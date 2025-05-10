using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Elara.ViewModel.Chat;

public partial class ChatViewModel : ObservableObject
{
    private readonly ConversationModel conversationModel;

    public ChatViewModel(ConversationModel conversationModel)
    {
        this.conversationModel = conversationModel;
    }

    [ObservableProperty]
    private string messageContent = string.Empty;

    [RelayCommand]
    private async Task SendMessageAsync()
    {
        await Task.CompletedTask;
    }
}
