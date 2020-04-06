namespace DotNetInterview.Services.Data.Helpers
{
    using System;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using DotNetInterview.Common;

    public static class Utils
    {
        public static string HideDelete(string itemUserId, string currentUserId, bool isAdmin)
        {
            return itemUserId == currentUserId || isAdmin ? string.Empty : "hidden";
        }

        public static string HideAddComment(string currentUserId)
        {
            return currentUserId != null ? string.Empty : "hidden";
        }

        public static bool IsModified(DateTime createdOn, DateTime? modifiedOn)
        {
            return modifiedOn != null && modifiedOn != createdOn ? true : false;
        }

        public static T SetStringValues<T>(T model, string inputField)
        {
            if (string.IsNullOrEmpty(inputField))
            {
                model.GetType()
               .GetProperty("GivenAnswerCss", BindingFlags.Public | BindingFlags.Instance)
               .SetValue(model, GlobalConstants.Hidden);

                model.GetType()
               .GetProperty("GivenAnswerBtnText", BindingFlags.Public | BindingFlags.Instance)
               .SetValue(model, GlobalConstants.AddAnswer);
            }
            else
            {
                model.GetType()
                .GetProperty("GivenAnswerCss", BindingFlags.Public | BindingFlags.Instance)
                .SetValue(model, string.Empty);

                model.GetType()
               .GetProperty("GivenAnswerBtnText", BindingFlags.Public | BindingFlags.Instance)
               .SetValue(model, GlobalConstants.DeleteAnswer);
            }

            return model;
        }

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
