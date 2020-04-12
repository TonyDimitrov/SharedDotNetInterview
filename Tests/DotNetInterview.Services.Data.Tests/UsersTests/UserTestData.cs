namespace DotNetInterview.Services.Data.Tests.UsersTests
{
    using DotNetInterview.Data.Models;

    public class UserTestData
    {
        public static ApplicationUser GetUserTestData()
        {
            return new ApplicationUser
            {
                Email = "toni@toni.com",
                PasswordHash = "AQAAAAEAACcQAAAAEDt5MojrolghU7VyfjhjsjX52RaGxtaTa0/n9LXcQ8gL54ihwg6UEcdkMj0ckE4jJw==",
                UserName = "toni@toni.com",
                FirstName = "Toni",
                LastName = "Dimitrov",
                IsDeleted = false,
                Image = "avatar",
            };
        }
    }
}
