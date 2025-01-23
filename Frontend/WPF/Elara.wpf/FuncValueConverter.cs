using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Elara.wpf;

/// <summary>
/// A general purpose <see cref="IValueConverter"/> that uses a <see cref="Func{TIn, TResult}"/>
/// to provide the converter logic.
/// </summary>
/// <typeparam name="TIn">The @in type.</typeparam>
/// <typeparam name="TOut">The @out type.</typeparam>
public class FuncValueConverter<TIn, TOut> : IValueConverter
{
    private readonly Func<TIn?, TOut?> _convert;
    private readonly Func<TOut?, TIn?>? _convertBack;

    /// <summary>
    /// Initializes a new instance of the <see cref="FuncValueConverter{TIn, TOut}"/> class.
    /// </summary>
    /// <param name="convert">The convert function.</param>
    public FuncValueConverter(Func<TIn?, TOut> convert, Func<TOut?, TIn>? convertBack = null)
    {
        _convert = convert;
        _convertBack = convertBack;
    }

    /// <inheritdoc/>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return _convert(default);
        }

        if (value is TIn @in)
        {
            return _convert(@in);
        }

        var valueType = value.GetType();
        var typeConverter = TypeDescriptor.GetConverter(valueType);
        if (!typeConverter.CanConvertFrom(valueType))
        {
            return DependencyProperty.UnsetValue;
        }

        return _convert((TIn?)typeConverter.ConvertFrom(value));
    }

    /// <inheritdoc/>
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (_convertBack == null)
        {
            return Binding.DoNothing;
        }

        if (value is null)
        {
            return _convertBack(default);
        }

        if (value is TOut @out)
        {
            return _convertBack(@out);
        }

        var valueType = value.GetType();
        var typeConverter = TypeDescriptor.GetConverter(valueType);
        if (!typeConverter.CanConvertFrom(valueType))
        {
            return Binding.DoNothing;
        }

        return _convertBack((TOut?)typeConverter.ConvertFrom(value));
    }
}

/// <summary>
/// A general purpose <see cref="IValueConverter"/> that uses a <see cref="Func{TIn, TParam, TOut}"/>
/// to provide the converter logic.
/// </summary>
/// <typeparam name="TIn">The @in type.</typeparam>
/// <typeparam name="TParam">The param type.</typeparam>
/// <typeparam name="TOut">The @out type.</typeparam>
public class FuncValueConverter<TIn, TParam, TOut> : IValueConverter
{
    private readonly Func<TIn?, TParam?, TOut?> _convert;
    private readonly Func<TOut?, TParam?, TIn?>? _convertBack;

    /// <summary>
    /// Initializes a new instance of the <see cref="FuncValueConverter{TIn, TParam, TOut}"/> class.
    /// </summary>
    /// <param name="convert">The convert function.</param>
    public FuncValueConverter(Func<TIn?, TParam?, TOut> convert, Func<TOut?, TParam?, TIn?>? convertBack = null)
    {
        _convert = convert;
        _convertBack = convertBack;
    }

    /// <inheritdoc/>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return _convert(default, (TParam?)parameter);
        }

        if (value is TIn @in)
        {
            return _convert(@in, (TParam?)parameter);
        }

        var valueType = value.GetType();
        var typeConverter = TypeDescriptor.GetConverter(valueType);
        if (!typeConverter.CanConvertFrom(valueType))
        {
            return DependencyProperty.UnsetValue;
        }

        return _convert((TIn?)typeConverter.ConvertFrom(value), (TParam?)parameter);
    }

    /// <inheritdoc/>
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (_convertBack == null)
        {
            return Binding.DoNothing;
        }

        if (value is null)
        {
            return _convertBack(default, (TParam?)parameter);
        }

        if (value is TOut @out)
        {
            return _convertBack(@out, (TParam?)parameter);
        }

        var valueType = value.GetType();
        var typeConverter = TypeDescriptor.GetConverter(valueType);
        if (!typeConverter.CanConvertFrom(valueType))
        {
            return Binding.DoNothing;
        }

        return _convertBack((TOut?)typeConverter.ConvertFrom(value), (TParam?)parameter);
    }
}