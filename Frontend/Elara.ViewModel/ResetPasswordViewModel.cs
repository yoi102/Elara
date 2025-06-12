using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Elara.ViewModel.Interfaces;
using Frontend.Shared.Identifiers;
using InteractionServices.Abstractions;
using Resources.Strings;
using Services.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Elara.wpf.ViewModel;

public partial class ResetPasswordViewModel : ObservableValidator, IHasCredentialsSubmitted
{
    private readonly IUserIdentityService userIdentityService;
    private readonly IDialogService dialogService;
    private readonly ISnackbarService snackbarService;

    public ResetPasswordViewModel(IUserIdentityService userIdentityService, IDialogService dialogService, ISnackbarService snackbarService)
    {
        this.userIdentityService = userIdentityService;
        this.dialogService = dialogService;
        this.snackbarService = snackbarService;
    }

    [Required(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "FieldRequired")]
    [EmailAddress(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "EmailAddressInvalid")]
    [NotifyDataErrorInfo]
    [ObservableProperty]
    private string email = string.Empty;

    [Required(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "FieldRequired")]
    [MinLength(1)]
    [NotifyDataErrorInfo]
    [ObservableProperty]
    private string resetCode = string.Empty;

    [Required(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "FieldRequired")]
    [MinLength(6, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "PasswordMinLength")]
    [RegularExpression(@"^[\u0021-\u007E]+$", ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "InvalidCharactersDetected")]
    [NotifyDataErrorInfo]
    [ObservableProperty]
    private string newPassword = string.Empty;

    [ObservableProperty]
    private bool canSendResetCode = true;

    public event EventHandler<AccountCredentialsEventArgs>? CredentialsSubmitted;

    protected virtual void OnCompleted(string nameOrEmail, string password)
    {
        CredentialsSubmitted?.Invoke(this, new AccountCredentialsEventArgs() { NameOrEmail = nameOrEmail, Password = password });
    }

    [RelayCommand]
    private async Task ResetAsync()
    {
        using var _ = dialogService.ShowProgressBarDialog(DialogHostIdentifiers.LoginRootDialog);

        ValidateAllProperties();
        if (HasErrors)
        {
            return;
        }

        await userIdentityService.ResetPasswordWithResetCodeAsync(Email, NewPassword, ResetCode);
        await Task.Delay(1000);//Simulate

        OnCompleted(Email, NewPassword);
    }

    [RelayCommand]
    private async Task SendResetCodeAsync()
    {
        using var _ = dialogService.ShowProgressBarDialog(DialogHostIdentifiers.LoginRootDialog);

        ValidateProperty(Email, nameof(Email));
        var errors = GetErrors(nameof(Email));
        if (errors.Any())
        {
            await dialogService.ShowOrReplaceMessageDialogAsync("Message:", Strings.PleaseEnterAValidEmailAddress, DialogHostIdentifiers.LoginRootDialog);
            return;
        }

        var response = await userIdentityService.GetResetCodeByEmailAsync(Email);
        await Task.Delay(1000);//Simulate
        CanSendResetCode = false;
        snackbarService.Enqueue(SnackBarHostIdentifiers.LoginWindow, $"(Email Message:)Reset Code：{response.ResetCode}", TimeSpan.FromSeconds(3));
        ResetCode = response.ResetCode;
    }
}
