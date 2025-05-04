namespace Elara.wpf.Converters;

public sealed class NullToBooleanConverter : NullConverter<bool>
{
    public static readonly NullToBooleanConverter FalseInstance = new() { FalseValue = false, TrueValue = true };
    public static readonly NullToBooleanConverter TrueInstance = new() { FalseValue = true, TrueValue = false };

    public NullToBooleanConverter() : base(true, false)
    {
    }
}
