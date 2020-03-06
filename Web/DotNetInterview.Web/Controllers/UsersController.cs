namespace DotNetInterview.Web.Controllers
{
    using System.Linq;

    using DotNetInterview.Data.Common.Repositories;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Web.ViewModels.Enums;
    using DotNetInterview.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly IDeletableEntityRepository<ApplicationUser> repository;

        public UsersController(UserManager<ApplicationUser> userManager, IDeletableEntityRepository<ApplicationUser> repository)
        {
            this.userManager = userManager;
            this.repository = repository;
        }

        public IActionResult Index()
        {
            var users = this.repository.All().ToList();
            var usersVM = users.Select(u => new UserVM
            {
                FullName = u.FirstName + " " + u.LastName,
                Position = (WorkPositionVM)u.Position,
                Email = u.Email,
            });

            return View(usersVM);
        }
    }
}