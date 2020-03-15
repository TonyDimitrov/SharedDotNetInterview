namespace DotNetInterview.Services.Data.Extensions
{
    using System;

    using DotNetInterview.Common;

    public static class DateTimeExtensions
    {
        public static string DateTimeViewFormater(
            this DateTime date,
            Func<DateTime, DateTime> timeZoneConverter = null,
            string format = GlobalConstants.FormatDate)
        {
            if (timeZoneConverter == null)
            {
                timeZoneConverter = dt => dt.ToLocalTime();
            }

            return timeZoneConverter(date).ToString(format);
        }
    }
}
