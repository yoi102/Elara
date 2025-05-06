using Elara.wpf.ViewModel;
using MahApps.Metro.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace Elara.wpf.View;

/// <summary>
/// Interaction logic for MainWindowView.xaml
/// </summary>
public partial class MainWindowView : MetroWindow
{
    public MainWindowView()
    {
        InitializeComponent();
        this.DataContext = App.Current.Services.GetService<MainWindowViewModel>();
    }
}
