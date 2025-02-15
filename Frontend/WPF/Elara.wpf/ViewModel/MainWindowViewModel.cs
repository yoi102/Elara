﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Service.Abstractions;

namespace Elara.wpf.ViewModel;

internal partial class MainWindowViewModel : ObservableObject
{
    private readonly IUserService userService;

    public MainWindowViewModel(IUserService userService)
    {
        this.userService = userService;
    }




    [RelayCommand]
    private async Task TestAsync()
    {
        var email = "string2@qwe.com";
        var result = await userService.GetEmailResetCodeAsync(email);
    }

}
