using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Elara.wpf.Assists;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elara.wpf.ViewModel
{
    internal partial class ResetPasswordViewModel : ObservableValidator
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


        [RelayCommand]
        private async Task ResetAsync()
        {
            dialogService.ShowProgressBarDialog(DialogHostIdentifiers.LoginRootDialog);

            ValidateAllProperties();
            if (HasErrors)
            {
                return;
            }


            var response = await userService.ResetPasswordWithEmailCodeAsync(Email, NewPassword, ResetCode);

            if (response.IsSuccessful)
            {
                await dialogService.TryShowMessageDialogAsync("Reset success", DialogHostIdentifiers.LoginRootDialog);
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

            ValidateAllProperties();
            var errors = GetErrors(nameof(Email));
            if (errors.Any())
            {
                await dialogService.TryShowMessageDialogAsync("Email!!!!", DialogHostIdentifiers.LoginRootDialog);
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

        }






    }
}
