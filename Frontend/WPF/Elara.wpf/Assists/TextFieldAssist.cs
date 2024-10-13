using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Elara.wpf.Assists
{
    public static class TextFieldAssist
    {
        public static readonly DependencyProperty IsSpaceInputBlockedProperty =
            DependencyProperty.RegisterAttached(
                "IsSpaceInputBlocked",
                typeof(bool),
                typeof(TextFieldAssist),
                new PropertyMetadata(false, OnIsSpaceInputBlockedChanged));

        public static bool GetIsSpaceInputBlocked(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsSpaceInputBlockedProperty);
        }

        public static void SetIsSpaceInputBlocked(DependencyObject obj, bool value)
        {
            obj.SetValue(IsSpaceInputBlockedProperty, value);
        }

        private static void OnIsSpaceInputBlockedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBoxBase textBox)
            {
                if ((bool)e.NewValue) // 如果值为 true，则附加阻止空格的行为
                {
                    textBox.PreviewKeyDown += TextField_PreviewKeyDown;
                }
                else
                {
                    textBox.PreviewKeyDown -= TextField_PreviewKeyDown;
                }
            }
            if (d is PasswordBox passwordBox)
            {
                if ((bool)e.NewValue) // 如果值为 true，则附加阻止空格的行为
                {
                    passwordBox.PreviewKeyDown += TextField_PreviewKeyDown;
                }
                else
                {
                    passwordBox.PreviewKeyDown -= TextField_PreviewKeyDown;
                }
            }
        }

        private static void TextField_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }


    }
}