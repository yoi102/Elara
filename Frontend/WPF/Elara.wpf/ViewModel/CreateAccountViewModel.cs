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
    internal partial class CreateAccountViewModel : ObservableValidator
    {
        private readonly IUserService userService;
        private readonly IDialogService dialogService;

        public CreateAccountViewModel(IUserService userService, IDialogService dialogService)
        {
            this.userService = userService;
            this.dialogService = dialogService;
        }


        [ObservableProperty]
        private string name = string.Empty;
        [EmailAddress]
        [ObservableProperty]
        private string email = string.Empty;
        [MinLength(6)]
        [ObservableProperty]
        private string password = string.Empty;


        [RelayCommand]
        private async Task CreateAsync()
        {
            ValidateAllProperties();
            if (HasErrors)
            {
                return;
            }
            dialogService.ShowProgressBarDialog(DialogHostIdentifiers.LoginRootDialog);
            await Task.Delay(1000);//Simulate

            var response = await userService.CreateAsync(Name, Email, Password);

            if (response.IsSuccessful)
            {
                await dialogService.TryShowMessageDialogAsync("Created", DialogHostIdentifiers.LoginRootDialog);

            }
            else 
            {
                await dialogService.TryShowMessageDialogAsync(response.ErrorMessage, DialogHostIdentifiers.LoginRootDialog);

            }


        }

    }
}
