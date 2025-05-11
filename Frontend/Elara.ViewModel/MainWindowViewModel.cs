using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Frontend.Shared.Identifiers;
using InteractionServices.Abstractions;
using Services.Abstractions;

namespace Elara.wpf.ViewModel;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly IConversationQueryService conversationQueryService;
    private readonly IDialogService dialogService;

    public MainWindowViewModel(IConversationQueryService conversationQueryService, IDialogService dialogService)
    {
        this.conversationQueryService = conversationQueryService;
        this.dialogService = dialogService;
    }
    public async Task InitializeAsync()
    {
        using var _ = dialogService.ShowProgressBarDialog(DialogHostIdentifiers.MainWindow);
        var conversationsData = await conversationQueryService.GetConversationsWithMessagesAsync();








    }

    [RelayCommand]
    private async Task TestAsync()
    {
        //强制退到登录界面
        //throw new ForceLogoutException();

        //Test
        await InitializeAsync();
        await Task.CompletedTask;
    }
}
