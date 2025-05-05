﻿using System.Windows;

namespace Elara.wpf.Converters;

public sealed class DefaultToVisibilityConverter : DefaultConverter<Visibility>
{
    public static readonly DefaultToVisibilityConverter CollapsedInstance = new() { NonDefaultValue = Visibility.Collapsed, DefaultValue = Visibility.Visible };
    public static readonly DefaultToVisibilityConverter NotCollapsedInstance = new() { NonDefaultValue = Visibility.Visible, DefaultValue = Visibility.Collapsed };

    public static readonly DefaultToVisibilityConverter HiddenInstance = new() { NonDefaultValue = Visibility.Hidden, DefaultValue = Visibility.Visible };
    public static readonly DefaultToVisibilityConverter NotHiddenInstance = new() { NonDefaultValue = Visibility.Visible, DefaultValue = Visibility.Hidden };

    public DefaultToVisibilityConverter() : base(Visibility.Collapsed, Visibility.Collapsed)
    {
    }
}
