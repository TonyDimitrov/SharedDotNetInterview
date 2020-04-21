namespace DotNetInterview.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using DotNetInterview.Common;
    using DotNetInterview.Data.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    internal class RolesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await SeedRoleAsync(roleManager, GlobalConstants.AdministratorRoleName);

            await SeedAdminUser(userManager);
        }

        private static async Task SeedRoleAsync(RoleManager<ApplicationRole> roleManager, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                var result = await roleManager.CreateAsync(new ApplicationRole(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
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
                Image = "avatar",
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
