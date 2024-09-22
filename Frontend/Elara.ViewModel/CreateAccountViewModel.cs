using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Elara.ViewModel.Interfaces;
using Services.Abstractions;
using System.ComponentModel.DataAnnotations;
using Frontend.Shared.Identifiers;
using Resources.Strings;
using InteractionServices.Abstractions;

namespace Elara.wpf.ViewModel;

public partial class CreateAccountViewModel : ObservableValidator, IHasCredentialsSubmitted
{
    private readonly IUserIdentityService userIdentityService;
    private readonly IDialogService dialogService;
    private readonly ISnackbarService snackbarService;

    [Required]
    [EmailAddress]
    [NotifyDataErrorInfo]
    [ObservableProperty]
    private string email = string.Empty;

    [Required]
    [MinLength(1)]
    [NotifyDataErrorInfo]
    [ObservableProperty]
    private string name = string.Empty;

    [Required]
    [MinLength(6)]
    [RegularExpression(@"^[\u0021-\u007E]+$")]
    [NotifyDataErrorInfo]
    [ObservableProperty]
    private string password = string.Empty;

    public CreateAccountViewModel(IUserIdentityService userIdentityService, IDialogService dialogService, ISnackbarService snackbarService)
    {
        this.userIdentityService = userIdentityService;
        this.dialogService = dialogService;
        this.snackbarService = snackbarService;
    }

    public event EventHandler<AccountCredentialsEventArgs>? CredentialsSubmitted;

    protected virtual void OnCreated(string nameOrEmail, string password)
    {
        CredentialsSubmitted?.Invoke(this, new AccountCredentialsEventArgs() { NameOrEmail = nameOrEmail, Password = password });
    }

    [RelayCommand]
    private async Task SignUpAsync()
    {
        ValidateAllProperties();
        if (HasErrors)
            return;

        using var _ = dialogService.ShowProgressBarDialog(DialogHostIdentifiers.LoginRootDialog);
        await Task.Delay(1000);//Simulate

        var response = await userIdentityService.SignUpAsync(Name, Email, Password);

        if (response.IsSuccessful)
        {
            snackbarService.Enqueue(SnackBarHostIdentifiers.LoginWindow, Strings.Created, TimeSpan.FromSeconds(2));
            OnCreated(Name, Password);
        }
        else
        {
            await dialogService.ShowMessageDialogAsync(response.ErrorMessage, DialogHostIdentifiers.LoginRootDialog);
        }
    }
}
