namespace DotNetInterview.Web.ViewModels.Users
{
    using System.Collections.Generic;

    public class UsersListVM
    {
        public IEnumerable<UserVM> Users { get; set; }
    }
}
