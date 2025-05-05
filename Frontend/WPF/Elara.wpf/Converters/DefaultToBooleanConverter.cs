namespace Elara.wpf.Converters;

public sealed class DefaultToBooleanConverter : DefaultConverter<bool>
{
    public static readonly DefaultToBooleanConverter FalseInstance = new() { NonDefaultValue = false, DefaultValue = true };
    public static readonly DefaultToBooleanConverter TrueInstance = new() { NonDefaultValue = true, DefaultValue = false };

    public DefaultToBooleanConverter() : base(true, false)
    {
    }
}
