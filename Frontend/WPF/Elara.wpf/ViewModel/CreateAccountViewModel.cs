using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Elara.wpf.Assists;
using Elara.wpf.Interfaces;
using Service.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Elara.wpf.ViewModel
{
    internal partial class CreateAccountViewModel : ObservableValidator, IHasCompleted
    {
        private readonly IDialogService dialogService;
        private readonly IUserService userService;
        [EmailAddress]
        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string name = string.Empty;

        [MinLength(6)]
        [ObservableProperty]
        private string password = string.Empty;

        public CreateAccountViewModel(IUserService userService, IDialogService dialogService)
        {
            this.userService = userService;
            this.dialogService = dialogService;
        }
        public event EventHandler<AccountEventArgs>? Completed;

        protected virtual void OnCompleted(string nameOrEmail, string password)
        {
            Completed?.Invoke(this, new AccountEventArgs() { NameOrEmail = nameOrEmail, Password = password });
        }

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
                OnCompleted(Name,Password);
            }
            else
            {
                await dialogService.TryShowMessageDialogAsync(response.ErrorMessage, DialogHostIdentifiers.LoginRootDialog);
            }
        }
    }
}