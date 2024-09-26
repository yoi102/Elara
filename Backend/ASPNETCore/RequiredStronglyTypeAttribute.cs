using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ASPNETCore
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class RequiredStronglyTypeAttribute : ValidationAttribute
    {
        //public const string DefaultErrorMessage = $"The {s} field must be {strong} type and the value not default value";

        public RequiredStronglyTypeAttribute()
        {
        }

        public Type StronglyType { get; set; } = typeof(Guid);
        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, $"The {name} field must be {StronglyType.Name} type and not default value");
        }

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return false;
            }

            var type = value.GetType();

            if (!type.IsValueType)
            {
                return false;
            }
            var info = type.GetProperty("Value");

            if (info is null)
            {
                return false;
            }
            var obj = info.GetValue(value);

            if (obj?.GetType() == StronglyType)
            {
                return obj != default;
            }

            return false;
        }
    }
}