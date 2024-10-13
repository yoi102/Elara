using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

        public CreateAccountViewModel(IUserService userService)
        {
            this.userService = userService;
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
          var result =  await userService.SignUpAsync(name, email, password);

        }

    }
}
