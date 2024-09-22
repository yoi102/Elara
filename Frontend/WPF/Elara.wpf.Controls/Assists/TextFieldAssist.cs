using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Elara.wpf.Controls.Assists;

public static class TextFieldAssist
{
    #region IsSpaceInputBlockedProperty

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
                textBox.PreviewKeyDown -= SpaceInputBlocked_PreviewKeyDown;
                textBox.PreviewKeyDown += SpaceInputBlocked_PreviewKeyDown;
            }
        }
        else if (d is PasswordBox passwordBox)
        {
            if ((bool)e.NewValue)
            {
                passwordBox.PreviewKeyDown -= SpaceInputBlocked_PreviewKeyDown;
                passwordBox.PreviewKeyDown += SpaceInputBlocked_PreviewKeyDown;
            }
        }
    }

    private static void SpaceInputBlocked_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Space)
        {
            e.Handled = true;
        }
    }

    #endregion IsSpaceInputBlockedProperty
}
