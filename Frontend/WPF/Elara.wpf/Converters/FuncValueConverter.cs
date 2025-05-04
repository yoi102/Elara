using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace Elara.wpf.Converters;

public sealed class FuncValueConverter<TIn, TOut> : IValueConverter
{
    private readonly Func<TIn, TOut> _convert;

    public FuncValueConverter(Func<TIn, TOut> convert)
    {
        _convert = convert;
    }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not TIn t)
        {
            if (value is null)
            {
                return default(TOut);
            }

            if (TypeDescriptor.GetConverter(typeof(TIn)).CanConvertFrom(value.GetType()))
            {
                t = (TIn)TypeDescriptor.GetConverter(typeof(TIn)).ConvertFrom(value)!;
            }
            else
            {
                return Binding.DoNothing;
            }
        }

        return _convert(t);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}
