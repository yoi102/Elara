using CommunityToolkit.Mvvm.Messaging;
using Elara.wpf.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
            PlayHideAnimationThenInvoke(() =>
            {
                WeakReferenceMessenger.Default.Unregister<LoginWindowViewModel>(this);
                DialogResult = true;
            });
        });
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

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed && e.OriginalSource is not TextBoxBase && e.OriginalSource is not PasswordBox)
        {
            this.DragMove();
        }
        base.OnMouseMove(e);
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        PlayHideAnimationThenInvoke(() =>
        {
            DialogResult = false;
        });
    }

    private void PlayHideAnimationThenInvoke(Action action)
    {
        var story = (Storyboard)FindResource("HideWindow");
        if (story == null)
            throw new ApplicationException();
        if (story.IsFrozen)
            story = story.Clone();
        story.Completed += delegate
        {
            action.Invoke();
        };
        story.Begin(this);
    }
}
