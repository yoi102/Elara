using CommunityToolkit.Mvvm.ComponentModel;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elara.wpf.ViewModel
{
    internal partial class LoginWindowViewModel:ObservableObject
    {
        private readonly IUserService userService;

        public LoginWindowViewModel(IUserService userService)
        {
            this.userService = userService;
        }











    }
}
