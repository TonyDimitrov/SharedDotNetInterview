namespace DotNetInterview.Web.Controllers
{
    using System.IO;

    using DotNetInterview.Common;
    using DotNetInterview.Services.Data;
    using DotNetInterview.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : BaseController
    {
        private readonly IUsersService usersService;
        private readonly IWebHostEnvironment hostingEnvironment;

        public UsersController(
            IUsersService usersService,
            IWebHostEnvironment hostingEnvironment)
        {
            this.usersService = usersService;
            this.hostingEnvironment = hostingEnvironment;
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

        public IActionResult UserAvatar(string imageName)
        {
            var imagePath = this.GetRootPath(this.hostingEnvironment, GlobalConstants.ImageFilesDirectory);

            if (!string.IsNullOrWhiteSpace(imageName))
            {
                var amagePathAndName = Path.Combine(imagePath, imageName);

                return this.PhysicalFile(amagePathAndName, this.BuildFileContenttype(imageName));
            }

            var amagePathAndDefaultName = Path.Combine(imagePath, GlobalConstants.DefaultAvatar);

            return this.PhysicalFile(amagePathAndDefaultName, this.BuildFileContenttype(GlobalConstants.DefaultAvatar));
        }
    }
}