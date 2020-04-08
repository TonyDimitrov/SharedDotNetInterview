namespace DotNetInterview.Services.Data.Extensions
{
    using System;

    using DotNetInterview.Common;

    public static class DateTimeExtensions
    {
        public static string DateTimeViewFormater(this DateTime date)
        {
            return date.ToLocalTime().ToString(GlobalConstants.FormatDate);
        }
    }
}
