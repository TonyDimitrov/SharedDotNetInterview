namespace DotNetInterview.Web.Infrastructure.Extensions
{
    using System.Linq;

    using DotNetInterview.Web.Infrastructure.CustomAttributes;

    public static class EnumDisplay
    {
        public static string DisplayName<TEnum>(this TEnum enumValue)
            where TEnum : struct
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())[0]
                .GetCustomAttributes(typeof(ViewDisplayAttribute), false)
                .Cast<ViewDisplayAttribute>()
                .FirstOrDefault()?.DisplayName;
        }

        public static string DisplayTooltipClass<TEnum>(this TEnum enumValue)
        where TEnum : struct
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())[0]
                .GetCustomAttributes(typeof(ViewTooltipAttribute), false)
                .Cast<ViewTooltipAttribute>()
                .FirstOrDefault()?.TooltipClass;
        }
    }
}