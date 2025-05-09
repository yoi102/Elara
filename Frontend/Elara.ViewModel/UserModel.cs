using CommunityToolkit.Mvvm.ComponentModel;

namespace Elara.ViewModel;

public partial class UserModel : ObservableObject
{
    [ObservableProperty]
    private readonly string? userName;

    [ObservableProperty]
    private readonly Uri? avatarUrl;
}
