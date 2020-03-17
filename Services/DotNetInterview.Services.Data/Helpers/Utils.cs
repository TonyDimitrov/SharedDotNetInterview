using System;

namespace DotNetInterview.Services.Data.Helpers
{
    public static class Utils
    {

        public static bool CanDelete(string itemUserId, string currentUserId, bool isAdmin)
        {
            return (itemUserId == currentUserId) || isAdmin;
        }

        public static bool CanAddComment(string currentUserId)
        {
            return currentUserId != null;
        }

        public static bool IsModified(DateTime createdOn, DateTime? modifiedOn)
        {
            return modifiedOn != null && modifiedOn != createdOn ? true : false;
        }
    }
}
