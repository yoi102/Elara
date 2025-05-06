using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Elara.wpf.ViewModel;

public partial class MainWindowViewModel : ObservableObject
{
    public MainWindowViewModel()
    {
    }

    [RelayCommand]
    private async Task TestAsync()
    {
        //Test

        await Task.CompletedTask;
    }
}
