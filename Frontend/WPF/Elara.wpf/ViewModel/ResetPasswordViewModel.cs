using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Elara.wpf.Assists;
using Elara.wpf.Interfaces;
using Service.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Elara.wpf.ViewModel
{
    internal partial class ResetPasswordViewModel : ObservableValidator, IHasCompleted
    {
        private readonly IUserService userService;
        private readonly IDialogService dialogService;

        public ResetPasswordViewModel(IUserService userService, IDialogService dialogService)
        {
            this.userService = userService;
            this.dialogService = dialogService;
        }


        [EmailAddress]
        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string resetCode = string.Empty;
        [MinLength(6)]
        [ObservableProperty]
        private string newPassword = string.Empty;

        public event EventHandler<AccountEventArgs>? Completed;

        protected virtual void OnCompleted(string nameOrEmail, string password)
        {
            Completed?.Invoke(this, new AccountEventArgs() { NameOrEmail = nameOrEmail, Password = password });
        }
        [RelayCommand]
        private async Task ResetAsync()
        {
            dialogService.ShowProgressBarDialog(DialogHostIdentifiers.LoginRootDialog);

            ValidateAllProperties();
            if (HasErrors)
            {
                dialogService.TryCloseDialog(DialogHostIdentifiers.LoginRootDialog);
                return;
            }


            var response = await userService.ResetPasswordWithEmailCodeAsync(Email, NewPassword, ResetCode);

            if (response.IsSuccessful)
            {
                await dialogService.TryShowMessageDialogAsync("Reset success", DialogHostIdentifiers.LoginRootDialog);
                OnCompleted(Email,NewPassword);
            }
            else
            {
                await dialogService.TryShowMessageDialogAsync(response.ErrorMessage, DialogHostIdentifiers.LoginRootDialog);

            }

        }
        [RelayCommand]
        private async Task SendResetCodeAsync()
        {
            dialogService.ShowProgressBarDialog(DialogHostIdentifiers.LoginRootDialog);

            ValidateProperty(Email, nameof(Email));
            var errors = GetErrors(nameof(Email));
            if (errors.Any())
            {
                await dialogService.TryShowMessageDialogAsync("Email!!!!", DialogHostIdentifiers.LoginRootDialog);
                return;
            }

            var response = await userService.GetEmailResetCodeAsync(Email);
            if (response.IsSuccessful)
            {
                //Do something


            }
            else
            {
                await dialogService.TryShowMessageDialogAsync(response.ErrorMessage, DialogHostIdentifiers.LoginRootDialog);


            }
            dialogService.TryCloseDialog(DialogHostIdentifiers.LoginRootDialog);
        }






    }
}
