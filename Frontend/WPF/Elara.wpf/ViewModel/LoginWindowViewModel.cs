using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Elara.wpf.Assists;
using Service.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Elara.wpf.ViewModel
{
    internal partial class LoginWindowViewModel : ObservableValidator
    {
        private readonly IDialogService dialogService;
        private readonly IUserService userService;
        [ObservableProperty]
        private ObservableObject? createOrReset;

        [ObservableProperty]
        private bool isLeftDrawerOpen = false;

        [Required]
        [NotifyDataErrorInfo]
        [ObservableProperty]
        private string nameEmail = string.Empty;

        [Required]
        [NotifyDataErrorInfo]
        [ObservableProperty]
        private string password = string.Empty;

        public LoginWindowViewModel(IUserService userService, IDialogService dialogService)
        {
            this.userService = userService;
            this.dialogService = dialogService;
        }

        [RelayCommand]
        private void Back()
        {
            CreateOrReset = null;
            IsLeftDrawerOpen = false;
        }

        [RelayCommand]
        private void Create()
        {
            CreateOrReset = new CreateAccountViewModel(userService);

            IsLeftDrawerOpen = true;
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            if (HasErrors)
                return;
            //using var _ = new ShowProgressBarDisposable(dialogService, DialogHostIdentifiers.LoginRootDialog);
            dialogService.ShowProgressBarDialog(DialogHostIdentifiers.LoginRootDialog);
            await Task.Delay(1000);

            //stringasdas@adawdaw.com
            //strqweasfewrqwing
            var login = await userService.LoginByNameAndPasswordAsync(NameEmail, Password);
            if (!login)
            {
                login = await userService.LoginByEmailAndPasswordAsync(NameEmail, Password);
            }
            if (!login)
            {
                await dialogService.TryShowMessageDialogAsync("密码或账号错误！！", DialogHostIdentifiers.LoginRootDialog);
            }
            else
            {
                var userInfo = await userService.GetUserInfoAsync();
                dialogService.TryCloseDialog(DialogHostIdentifiers.LoginRootDialog);
                WeakReferenceMessenger.Default.Send(this);
            }
        }

        [RelayCommand]
        private void Reset()
        {
            CreateOrReset = new ResetPasswordViewModel(userService);

            IsLeftDrawerOpen = true;
        }
    }
}