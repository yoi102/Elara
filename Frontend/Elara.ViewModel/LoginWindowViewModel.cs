using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Elara.ViewModel.Interfaces;
using Frontend.Shared.Identifiers;
using InteractionServices.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace Elara.wpf.ViewModel;

public partial class LoginWindowViewModel : ObservableValidator
{
    private readonly ICultureSettingService cultureSettingService;
    private readonly IDialogService dialogService;
    private readonly IServiceProvider serviceProvider;
    private readonly IThemeSettingService themeSettingService;
    private readonly IUserIdentityService userIdentityService;
    private IHasCredentialsSubmitted? createOrReset;

    [ObservableProperty]
    private int currentCultureLCID;

    private bool isDarkTheme;

    [Required]
    [NotifyDataErrorInfo]
    [ObservableProperty]
    private string nameEmail = string.Empty;

    [Required]
    [MinLength(6)]
    [RegularExpression(@"^[\u0021-\u007E]+$")]
    [NotifyDataErrorInfo]
    [ObservableProperty]
    private string password = string.Empty;

    public LoginWindowViewModel(IServiceProvider serviceProvider,
        IUserIdentityService userIdentityService,
        IDialogService dialogService,
        ICultureSettingService cultureSettingService,
        IThemeSettingService themeSettingService)
    {
        this.serviceProvider = serviceProvider;
        this.userIdentityService = userIdentityService;
        this.dialogService = dialogService;
        this.cultureSettingService = cultureSettingService;
        this.themeSettingService = themeSettingService;
        currentCultureLCID = System.Globalization.CultureInfo.CurrentCulture.LCID;
        isDarkTheme = themeSettingService.IsDarkTheme;
    }

    public IHasCredentialsSubmitted? SignUpOrResetViewModel
    {
        get { return createOrReset; }
        set
        {
            if (value == null && createOrReset != null)
            {
                createOrReset.CredentialsSubmitted -= OnCreateOrResetCompleted;
            }
            if (value != null)
            {
                value.CredentialsSubmitted += OnCreateOrResetCompleted;
            }
            SetProperty(ref createOrReset, value);
        }
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

    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            var address = new MailAddress(email);
            return address.Address == email;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    [RelayCommand]
    private void Back()
    {
        SignUpOrResetViewModel = null;
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
    private void SignUp()
    {
        SignUpOrResetViewModel = serviceProvider.GetService<SignUpViewModel>();
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        ValidateAllProperties();
        if (HasErrors)
            return;

        using var _ = dialogService.ShowProgressBarDialog(DialogHostIdentifiers.LoginRootDialog);
        await Task.Delay(1000);//Simulate

        var userData = await userIdentityService.LoginByNameAndPasswordAsync(NameEmail, Password);
        if (userData is null)
        {
            if (IsValidEmail(NameEmail))
                userData = await userIdentityService.LoginByEmailAndPasswordAsync(NameEmail, Password);
        }
        if (userData is null)
        {
            await dialogService.ShowOrReplaceMessageDialogAsync("Incorrect username or password.", DialogHostIdentifiers.LoginRootDialog);
        }
        else
        {
            WeakReferenceMessenger.Default.Send(this);
        }
    }

    private void OnCreateOrResetCompleted(object? sender, AccountCredentialsEventArgs accountEventArgs)
    {
        SignUpOrResetViewModel = null;
        NameEmail = accountEventArgs.NameOrEmail;
        Password = accountEventArgs.Password;
    }

    [RelayCommand]
    private void Reset()
    {
        SignUpOrResetViewModel = serviceProvider.GetService<ResetPasswordViewModel>();
    }
}
