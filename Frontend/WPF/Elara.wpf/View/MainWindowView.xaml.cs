using Elara.wpf.ViewModel;
using MahApps.Metro.Controls;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;

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

    private void ColorZone_MouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            this.DragMove();
        }
    }
}
