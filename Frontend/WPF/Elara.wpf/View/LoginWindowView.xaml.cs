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


        EventManager.RegisterClassHandler(typeof(TextBoxBase),
            UIElement.MouseMoveEvent, new MouseEventHandler(PreventMouseMoveEventBubbling));
        EventManager.RegisterClassHandler(typeof(PasswordBox),
            UIElement.MouseMoveEvent, new MouseEventHandler(PreventMouseMoveEventBubbling));


        WeakReferenceMessenger.Default.Register<LoginWindowViewModel>(this, (r, m) =>
        {

            PlayHideAnimationThenInvoke(() =>
            {
                WeakReferenceMessenger.Default.Unregister<LoginWindowViewModel>(this);
                DialogResult = true;

            });
        });

    }
    private void PreventMouseMoveEventBubbling(object sender, MouseEventArgs e)
    {
        e.Handled = true;
    }


    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
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
        //var story = (Storyboard)Application.Current.Resources["HideWindow"];
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
