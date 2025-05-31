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
    private readonly ICultureSettingService cultureSettingService;
    private readonly IDialogService dialogService;
    private readonly IPersonalSpaceProfileService personalSpaceProfileService;
    private readonly IServiceProvider serviceProvider;
    private readonly IThemeSettingService themeSettingService;
    [ObservableProperty]
    private ChatShellViewModel chatShellViewModel;

    [ObservableProperty]
    private ContactShellViewModel contactShellViewModel;

    [ObservableProperty]
    private int currentCultureLCID;

    [ObservableProperty]
    private object? currentShellViewModel;

    private bool isDarkTheme;

    [ObservableProperty]
    private UserProfileData? userInfo;

    public MainWindowViewModel(IServiceProvider serviceProvider,
                                IDialogService dialogService,
        IPersonalSpaceProfileService personalSpaceProfileService,
        IThemeSettingService themeSettingService,
        ICultureSettingService cultureSettingService)
    {
        this.serviceProvider = serviceProvider;
        this.dialogService = dialogService;
        this.personalSpaceProfileService = personalSpaceProfileService;
        this.themeSettingService = themeSettingService;
        this.cultureSettingService = cultureSettingService;
        chatShellViewModel = serviceProvider.GetService<ChatShellViewModel>()!;
        contactShellViewModel = serviceProvider.GetService<ContactShellViewModel>()!;
        currentCultureLCID = System.Globalization.CultureInfo.CurrentCulture.LCID;
        isDarkTheme = themeSettingService.IsDarkTheme;
    }
    public bool IsDarkTheme
    {
        get { return isDarkTheme; }
        set
        {
            if (SetProperty(ref isDarkTheme, value))
            {
                themeSettingService.ApplyThemeLightDark(value);
            }
        }
    }
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
    private void ChangeCulture(string lcidString)
    {
        if (!int.TryParse(lcidString, out var lcid))
            return;
        CurrentCultureLCID = lcid;
        cultureSettingService.ChangeCulture(lcid);
    }

    [RelayCommand]
    private void ChangeShellViewModel(object shellViewModel)
    {
        using var _ = dialogService.ShowProgressBarDialog(DialogHostIdentifiers.MainShellControl);

        CurrentShellViewModel = shellViewModel;
        //刷新数据？
    }
}
