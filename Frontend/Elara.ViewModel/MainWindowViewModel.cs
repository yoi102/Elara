using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Elara.ViewModel.Chat;
using Elara.ViewModel.Contact;
using Frontend.Shared.Identifiers;
using InteractionServices.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Elara.wpf.ViewModel;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly IServiceProvider serviceProvider;
    private readonly IDialogService dialogService;

    public MainWindowViewModel(IServiceProvider serviceProvider, IDialogService dialogService)
    {
        this.serviceProvider = serviceProvider;
        this.dialogService = dialogService;
        chatShellViewModel = serviceProvider.GetService<ChatShellViewModel>()!;
        contactShellViewModel = serviceProvider.GetService<ContactShellViewModel>()!;
    }
    [ObservableProperty]
    private ChatShellViewModel chatShellViewModel;

    [ObservableProperty]
    private ContactShellViewModel contactShellViewModel;



    public async Task InitializeAsync()
    {
        using var _ = dialogService.ShowProgressBarDialog(DialogHostIdentifiers.MainWindow);
        await ChatShellViewModel.InitializeAsync();
        await ContactShellViewModel.InitializeAsync();









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
