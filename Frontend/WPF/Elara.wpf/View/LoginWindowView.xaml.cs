using CommunityToolkit.Mvvm.Messaging;
using Elara.wpf.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Elara.wpf.View;

/// <summary>
/// LoginWindowView.xaml 的交互逻辑
/// </summary>
public partial class LoginWindowView : Window
{
    public LoginWindowView()
    {
        InitializeComponent();
        this.DataContext = App.Current.Services.GetService<LoginWindowViewModel>();

        WeakReferenceMessenger.Default.Register<LoginWindowViewModel>(this, (r, m) =>
        {
            DialogResult = true;
        });
        WindowTracker.Register(this);
        this.Closing += LoginWindowView_Closing;
    }

    private bool _isClosing = false;
    private bool? _dialogResult;

    private void LoginWindowView_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        if (_isClosing)
            return;
        // 第一次进来，阻止关闭
        e.Cancel = true;
        _isClosing = true;
        _dialogResult = DialogResult;
        var story = (Storyboard)FindResource("HideWindow") ?? throw new ApplicationException();
        if (story.IsFrozen)
            story = story.Clone();

        story.Completed += (s, a) =>
        {
            DialogResult = _dialogResult;
        };
        story.Begin(this);
    }

    public static FuncValueConverter<int, string> LCIDToStringConverter { get; } = new(lcid =>
    {
        var result = "EN";
        if (lcid == 2052)
        {
            result = "CN";
        }
        else if (lcid == 1041)
        {
            result = "JP";
        }
        else if (lcid == 1033)
        {
            result = "EN";
        }
        return result;
    });

    private void DragBorder_MouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
            if (e.OriginalSource == sender)
                this.DragMove();
    }

    private void CloseButtonClicked(object sender, RoutedEventArgs e)
    {
       DialogResult = false;
    }
}
