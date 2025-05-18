namespace Elara.wpf.Converters;

public sealed class DefaultToBooleanConverter : DefaultConverter<bool>
{
    public static readonly DefaultToBooleanConverter DefaultToTrueInstance = new() { NonDefaultValue = false, DefaultValue = true };
    public static readonly DefaultToBooleanConverter DefaultToFalseInstance = new() { NonDefaultValue = true, DefaultValue = false };

    public DefaultToBooleanConverter() : base(true, false)
    {
    }
}
