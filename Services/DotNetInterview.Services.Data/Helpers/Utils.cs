namespace DotNetInterview.Services.Data.Helpers
{
    using DotNetInterview.Common;
    using System;

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

        public static void SetStringValues(string inputField, string css, string text)
        {
            if (string.IsNullOrEmpty(inputField))
            {
                css = "hidden";
                text = GlobalConstants.AddAnswer;
            }
            else
            {
                css = string.Empty;
                text = GlobalConstants.DeleteAnswer;
            }
        }
    }
}
