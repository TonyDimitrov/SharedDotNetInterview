using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DotNetInterview.Common
{
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
