namespace DotNetInterview.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using DotNetInterview.Common;
    using DotNetInterview.Data.Common.Repositories;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Services.Data;
    using DotNetInterview.Web.ViewModels.Settings;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class SettingsController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        private readonly ISettingsService settingsService;
        private readonly IDeletableEntityRepository<Setting> repository;

        public SettingsController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            ISettingsService settingsService,
            IDeletableEntityRepository<Setting> repository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.settingsService = settingsService;
            this.repository = repository;
        }

        public IActionResult Index()
        {
            var settings = this.settingsService.GetAll<SettingViewModel>();
            var model = new SettingsListViewModel { Settings = settings };
            return this.View(model);
        }

        public async Task<IActionResult> InsertSetting()
        {
            var random = new Random();
            var setting = new Setting { Name = $"Name_{random.Next()}", Value = $"Value_{random.Next()}" };

            await this.repository.AddAsync(setting);
            await this.repository.SaveChangesAsync();

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> DeleteSetting(SettingViewModel model)
        {
            this.repository.Delete(new Setting { Id = model.Id });
            await this.repository.SaveChangesAsync();

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> CreateUser()
        {
            var user = new ApplicationUser
            {
                Email = "tosho@tosh.com",
                UserName = "tosho@tosh.com",
                FirstName = "Tosho",
                LastName = "Toshev",
                IsDeleted = false,
                Image = "avatar",
            };

            var create = await this.userManager.CreateAsync(user, "123456");

            var signIn = await this.signInManager.PasswordSignInAsync(user, "123456", true, false);
            if (signIn.Succeeded)
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            return this.BadRequest("Could not sign in!");
        }

        public async Task<IActionResult> CreateRole()
        {
            var role = await this.roleManager.CreateAsync(new ApplicationRole { Name = GlobalConstants.AdministratorRoleName });
            if (role.Succeeded)
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            return this.BadRequest("Could not create role!");
        }

        public async Task<IActionResult> SetRole()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var role = await this.userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
            if (role.Succeeded)
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            return this.BadRequest("Could not create role!");
        }
    }
}
