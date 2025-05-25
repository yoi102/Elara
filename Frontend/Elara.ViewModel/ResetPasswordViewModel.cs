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

    [Required]
    [EmailAddress]
    [NotifyDataErrorInfo]
    [ObservableProperty]
    private string email = string.Empty;

    [Required]
    [MinLength(1)]
    [NotifyDataErrorInfo]
    [ObservableProperty]
    private string resetCode = string.Empty;

    [Required]
    [MinLength(6)]
    [RegularExpression(@"^[\u0021-\u007E]+$")]
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
            await dialogService.ShowOrReplaceMessageDialogAsync(Strings.PleaseEnterAValidEmailAddress, DialogHostIdentifiers.LoginRootDialog);
            return;
        }

        var response = await userIdentityService.GetResetCodeByEmailAsync(Email);
        await Task.Delay(1000);//Simulate
        CanSendResetCode = false;
        snackbarService.Enqueue(SnackBarHostIdentifiers.LoginWindow, $"(Email Message:)Reset Code：{response.ResetCode}", TimeSpan.FromSeconds(3));
        ResetCode = response.ResetCode;
    }
}
