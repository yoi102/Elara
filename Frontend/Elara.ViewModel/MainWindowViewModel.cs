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

    [ObservableProperty]
    private object? currentShellViewModel;

    public async Task InitializeAsync()
    {
        using var _ = dialogService.ShowProgressBarDialog(DialogHostIdentifiers.MainWindow);
        await ChatShellViewModel.InitializeAsync();
        await ContactShellViewModel.InitializeAsync();

        CurrentShellViewModel = ChatShellViewModel;
        CurrentShellViewModel = ContactShellViewModel;
    }

    [RelayCommand]
    private void ChangeShellViewModel(object shellViewModel)
    {
        using var _ = dialogService.ShowProgressBarDialog(DialogHostIdentifiers.MainContentControl);

        CurrentShellViewModel = shellViewModel;
        //刷新数据？
    }
}
