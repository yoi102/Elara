using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Elara.wpf.Assists;

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
            if ((bool)e.NewValue)
            {
                textBox.PreviewKeyDown += TextField_PreviewKeyDown;
            }
            else
            {
                textBox.PreviewKeyDown -= TextField_PreviewKeyDown;
            }
        }
        else if (d is PasswordBox passwordBox)
        {
            if ((bool)e.NewValue)
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