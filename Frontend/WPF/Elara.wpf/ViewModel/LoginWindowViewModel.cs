using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Elara.wpf.ViewModel
{
    internal partial class LoginWindowViewModel : ObservableObject
    {
        private readonly IUserService userService;

        public LoginWindowViewModel(IUserService userService)
        {
            this.userService = userService;
        }


        [ObservableProperty]
        private string nameEmail = string.Empty;
        [ObservableProperty]
        private string password = string.Empty;
        [ObservableProperty]
        private bool isLeftDrawerOpen = false;
        [ObservableProperty]
        private ObservableObject? createOrReset;


        [RelayCommand]
        private void Create()
        {
            CreateOrReset = new CreateAccountViewModel(userService);

            IsLeftDrawerOpen = true;

        }

        [RelayCommand]
        private void Reset()
        {
            CreateOrReset = new ResetPasswordViewModel(userService);

            IsLeftDrawerOpen = true;



        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            try
            {
                //stringasdas@adawdaw.com
                //    strqweasfewrqwing
                var login = await userService.LoginByNameAndPasswordAsync(nameEmail, password);
                if (!login)
                {
                    login = await userService.LoginByEmailAndPasswordAsync(nameEmail, password);
                    var userInfo = await userService.GetUserInfoAsync();
                }
                if (!login)
                {
                    //do something.


                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }



        [RelayCommand]
        private void Back()
        {
            CreateOrReset = null;
            IsLeftDrawerOpen = false;


        }









    }
}
