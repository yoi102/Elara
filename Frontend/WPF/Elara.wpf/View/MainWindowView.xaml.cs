using Elara.wpf.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Elara.wpf.View;

/// <summary>
/// Interaction logic for MainWindowView.xaml
/// </summary>
public partial class MainWindowView : Window
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

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        var story = (Storyboard)FindResource("HideWindow");
        //var story = (Storyboard)Application.Current.Resources["HideWindow"];
        if (story == null)
            throw new ApplicationException();
        if (story.IsFrozen)
            story = story.Clone();
        story.Completed += delegate
        {
            this.Close();
        };
        story.Begin(this);
    }

    private void MinimizeButton_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }



    private void MaximizeButton_Click(object sender, RoutedEventArgs e)
    {

        if (WindowState == WindowState.Maximized)
        {
            WindowState = WindowState.Normal;
            MaximizeButton.ToolTip = "Restore down";
        }
        else
        {
            WindowState = WindowState.Maximized;
            MaximizeButton.ToolTip = "Maximize";

        }


    }
}