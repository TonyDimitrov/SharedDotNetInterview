namespace DotNetInterview.Web.ViewModels.Administration.Users
{
    using System.Collections.Generic;

    using DotNetInterview.Web.ViewModels.Common;

    public class DeletedUsersVM : PaginationVM
    {
        public IEnumerable<DeletedUserVM> DeletedUsers { get; set; }
    }
}
