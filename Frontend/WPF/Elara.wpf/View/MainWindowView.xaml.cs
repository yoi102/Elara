using Elara.wpf.ViewModel;
using MahApps.Metro.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace Elara.wpf.View;

/// <summary>
/// Interaction logic for MainWindowView.xaml
/// </summary>
public partial class MainWindowView : MetroWindow
{
    private MainWindowViewModel? _vm;

    public MainWindowView()
    {
        InitializeComponent();
        this.DataContext = _vm = App.Current.Services.GetService<MainWindowViewModel>();
        this.ContentRendered += MainWindowView_ContentRendered;
    }

    private async void MainWindowView_ContentRendered(object? sender, EventArgs e)
    {

        if (_vm is not null)
            await _vm.InitializeAsync();
    }


}
