using System;

namespace DotNetInterview.Services.Data.Helpers
{
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
    }
}
