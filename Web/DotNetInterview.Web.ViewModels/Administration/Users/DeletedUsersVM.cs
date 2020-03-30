namespace DotNetInterview.Web.ViewModels.Administration.Users
{
    using System.Collections.Generic;

    public class DeletedUsersVM
    {
        public IEnumerable<DeletedUserVM> DeletedUsers { get; set; }
    }
}
