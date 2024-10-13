using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elara.wpf.ViewModel
{
    internal partial class ResetPasswordViewModel : ObservableObject
    {
        private readonly IUserService userService;

        public ResetPasswordViewModel(IUserService userService)
        {
            this.userService = userService;
        }



        [ObservableProperty]
        private string email = string.Empty;
        [ObservableProperty]
        private string resetCode = string.Empty;

        [ObservableProperty]
        private string newPassword = string.Empty;


        [RelayCommand]
        private async Task ResetAsync()
        {
            var result = await userService.ResetPasswordWithEmailCodeAsync(email, newPassword, resetCode);

        }
        [RelayCommand]
        private async Task SendResetCodeAsync()
        {
            var result = await userService.GetEmailResetCodeAsync(email);
            if (result)
            {
                ResetCode = "123";
            }

        }






    }
}
