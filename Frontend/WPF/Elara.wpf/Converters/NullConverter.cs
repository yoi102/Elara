using System.Globalization;
using System.Windows.Data;

namespace Elara.wpf.Converters;

public class NullConverter<T>(T trueValue, T falseValue) : IValueConverter
{
    public T TrueValue { get; set; } = trueValue;
    public T FalseValue { get; set; } = falseValue;

    public virtual object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is null ? TrueValue : FalseValue;

    public virtual object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => Binding.DoNothing;
}
