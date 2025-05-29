using ApiClients.Abstractions.Models.Responses;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Elara.ViewModel.Chat;
using Elara.ViewModel.Contact;
using Frontend.Shared.Identifiers;
using InteractionServices.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions.PersonalSpaceServices;

namespace Elara.wpf.ViewModel;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly IServiceProvider serviceProvider;
    private readonly IDialogService dialogService;
    private readonly IPersonalSpaceProfileService personalSpaceProfileService;

    public MainWindowViewModel(IServiceProvider serviceProvider, IDialogService dialogService, IPersonalSpaceProfileService personalSpaceProfileService)
    {
        this.serviceProvider = serviceProvider;
        this.dialogService = dialogService;
        this.personalSpaceProfileService = personalSpaceProfileService;
        chatShellViewModel = serviceProvider.GetService<ChatShellViewModel>()!;
        contactShellViewModel = serviceProvider.GetService<ContactShellViewModel>()!;
    }

    [ObservableProperty]
    private ChatShellViewModel chatShellViewModel;

    [ObservableProperty]
    private ContactShellViewModel contactShellViewModel;

    [ObservableProperty]
    private object? currentShellViewModel;

    [ObservableProperty]
    private UserProfileData? userInfo;

    public async Task InitializeAsync()
    {
        using var _ = dialogService.ShowProgressBarDialog(DialogHostIdentifiers.MainWindow);
        await ChatShellViewModel.InitializeAsync();
        await ContactShellViewModel.InitializeAsync();

        CurrentShellViewModel = ChatShellViewModel;
        CurrentShellViewModel = ContactShellViewModel;
        UserInfo = await personalSpaceProfileService.GetCurrentUserProfileAsync();
    }

    [RelayCommand]
    private void ChangeShellViewModel(object shellViewModel)
    {
        using var _ = dialogService.ShowProgressBarDialog(DialogHostIdentifiers.MainShellControl);

        CurrentShellViewModel = shellViewModel;
        //刷新数据？
    }
}
