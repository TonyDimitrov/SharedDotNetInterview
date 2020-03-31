namespace DotNetInterview.Web.ViewModels.Users
{
    using System.Collections.Generic;

    public class AllUsersVM
    {
        public IEnumerable<UserVM> Users { get; set; }
    }
}
