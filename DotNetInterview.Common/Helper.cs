namespace DotNetInterview.Common
{
    using System.Text.RegularExpressions;

    public static class Helper
    {
        public static string ParseEnum<T>(T @enum, string separator = " ")
            where T : struct
        {
            var r = new Regex(
                @"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

            return r.Replace(@enum.ToString(), separator);
        }
    }
}
