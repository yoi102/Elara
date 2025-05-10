using CommunityToolkit.Mvvm.ComponentModel;

namespace Elara.ViewModel;

public partial class UserModel : ObservableObject
{
    [ObservableProperty]
    private string? userName;

    [ObservableProperty]
    private Uri? avatarUrl;
}
