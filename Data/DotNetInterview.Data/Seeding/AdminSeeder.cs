namespace DotNetInterview.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    using DotNetInterview.Common;
    using DotNetInterview.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public class AdminSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await SeedAdminUser(userManager);
        }

        private static async Task SeedAdminUser(UserManager<ApplicationUser> userManager)
        {
            var user = new ApplicationUser
            {
                Email = "toni@dimitrov",
                UserName = "toni@dimitrov",
                FirstName = "Ton",
                LastName = "Anton",
                IsDeleted = false,
                Image = "default-avatar.jpg",
            };

            var userIsInDb = await userManager.FindByNameAsync(user.UserName);
            if (userIsInDb != null)
            {
                var isinRole = await userManager.IsInRoleAsync(userIsInDb, GlobalConstants.AdministratorRoleName);
                if (isinRole)
                {
                    return;
                }

                await userManager.AddToRoleAsync(userIsInDb, GlobalConstants.AdministratorRoleName);

                return;
            }

            var result = await userManager.CreateAsync(user, "123456");
            if (result.Succeeded)
            {
                userIsInDb = await userManager.FindByNameAsync(user.UserName);
                await userManager.AddToRoleAsync(userIsInDb, GlobalConstants.AdministratorRoleName);
            }
        }
    }
}
