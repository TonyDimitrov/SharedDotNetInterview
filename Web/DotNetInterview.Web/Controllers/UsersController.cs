namespace DotNetInterview.Web.Controllers
{
    using System.Linq;

    using DotNetInterview.Data.Common.Repositories;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Services;
    using DotNetInterview.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUsersService usersService;
        private readonly IDeletableEntityRepository<ApplicationUser> repository;

        public UsersController(UserManager<ApplicationUser> userManager, IUsersService usersService)
        {
            this.userManager = userManager;
            this.usersService = usersService;
        }

        //public IActionResult Index()
        //{
        //    var users = this.repository.All().ToList();
        //    var usersVM = users.Select(u => new UserVM
        //    {
        //        FullName = u.FirstName + " " + u.LastName,
        //        Position = (WorkPositionVM)u.Position,
        //        Email = u.Email,
        //    });

        //    return this.View(usersVM);
        //}

        public IActionResult Details(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return this.Redirect("/Interviews/All");
            }

            var userDetails = this.usersService.Details<DetailsUserVM>(userId);

            return this.View(userDetails);
        }
    }
}