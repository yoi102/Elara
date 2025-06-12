using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Elara.ViewModel.Interfaces;
using Frontend.Shared.Identifiers;
using InteractionServices.Abstractions;
using Resources.Strings;
using Services.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Elara.wpf.ViewModel;

public partial class SignUpViewModel : ObservableValidator, IHasCredentialsSubmitted
{
    private readonly IUserIdentityService userIdentityService;
    private readonly IDialogService dialogService;
    private readonly ISnackbarService snackbarService;

    [Required]
    [EmailAddress(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "EmailAddressInvalid")]
    [NotifyDataErrorInfo]
    [ObservableProperty]
    private string email = string.Empty;

    [Required(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "FieldRequired")]
    [MinLength(1)]
    [NotifyDataErrorInfo]
    [ObservableProperty]
    private string name = string.Empty;

    [Required(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "FieldRequired")]
    [MinLength(6, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "PasswordMinLength")]
    [RegularExpression(@"^[\u0021-\u007E]+$", ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "InvalidCharactersDetected")]
    [NotifyDataErrorInfo]
    [ObservableProperty]
    private string password = string.Empty;

    public SignUpViewModel(IUserIdentityService userIdentityService, IDialogService dialogService, ISnackbarService snackbarService)
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

        await userIdentityService.SignUpAsync(Name, Email, Password);
        await Task.Delay(1000);//Simulate

        snackbarService.Enqueue(SnackBarHostIdentifiers.LoginWindow, Strings.Created, TimeSpan.FromSeconds(2));
        OnCreated(Name, Password);
    }
}
