namespace DotNetInterview.Web.Controllers
{
    using DotNetInterview.Data.Common.Repositories;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Web.ViewModels.Enums;
    using DotNetInterview.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;

    public class UsersController : Controller
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
                Position = (WorkPosition)u.Position,
                Country = (Country)u.Country,
                Email = u.Email,
            });

            return View(usersVM);
        }
    }
}