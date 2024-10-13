using Elara.wpf.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Elara.wpf.View
{
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
            PlayHideAnimationThenInvoke(() => { DialogResult = false; });
        }

        private void PlayHideAnimationThenInvoke(Action action)
        {
            var story = (Storyboard)this.Resources["HideWindow"];
            if (story == null)
                throw new ApplicationException();
            story.Completed += delegate
            {
                action.Invoke();
            };
            story.Begin(this);
        }

    }
}
