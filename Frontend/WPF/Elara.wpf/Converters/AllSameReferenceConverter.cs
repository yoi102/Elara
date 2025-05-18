using System.Globalization;
using System.Windows.Data;

namespace Elara.wpf.Converters;

public class AllSameReferenceConverter : IMultiValueConverter
{
    public static readonly AllSameReferenceConverter Instance = new();

    public object? Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values == null || values.Length == 0)
            return false;

        var first = values[0];
        return values.All(v => ReferenceEquals(v, first));
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
