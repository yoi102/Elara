using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ASPNETCore;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class RequiredGuidStronglyIdAttribute : ValidationAttribute
{
    public RequiredGuidStronglyIdAttribute()
    {
    }
    public override string FormatErrorMessage(string name)
    {
        return string.Format(CultureInfo.CurrentCulture, $"The {name} field must be {nameof(Guid)} type and not default value");
    }

    public override bool IsValid(object? value)
    {
        if (value == null)
        {
            return false;
        }

        var valueString = value.ToString();

        if (string.IsNullOrEmpty(valueString))
            return false;

        var guid = new Guid(valueString);

        if (guid == Guid.Empty)
            return false;
        return true;

    }
}