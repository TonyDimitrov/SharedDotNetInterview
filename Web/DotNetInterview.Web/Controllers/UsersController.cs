namespace DotNetInterview.Web.Controllers
{
    using System.IO;
    using System.Threading.Tasks;

    using DotNetInterview.Common;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Services.Data;
    using DotNetInterview.Web.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : BaseController
    {
        private readonly IUsersService usersService;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IWebHostEnvironment hostingEnvironment;

        public UsersController(
            IUsersService usersService,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment hostingEnvironment)
        {
            this.usersService = usersService;
            this.signInManager = signInManager;
            this.hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Details(string userId)
        {
            var isLoggedInUser = userId == this.GetLoggedInUserId(this.User);

            var userDetails = this.usersService.Details(userId, isLoggedInUser, this.IsAdmin());

            if (userDetails == null)
            {
                var errorVM = new ItemNotFoundErrorVM
                {
                    ItemId = userId,
                    Message = string.Format(ErrorMessages.ItemNotFound, "User", userId),
                    RequestUrl = this.HttpContext.Request.GetDisplayUrl(),
                };

                return this.RedirectToAction("ItemNotFound", "NotFound", errorVM);
            }

            return this.View(userDetails);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(string userId)
        {
            var cuurentLoggedInUserId = this.GetLoggedInUserId(this.User);
            var isCurrentLoogedInUserAdmin = this.IsAdmin();

            if (cuurentLoggedInUserId == userId)
            {
                await this.usersService.Delete(userId);
                await this.signInManager.SignOutAsync();

                return this.Redirect("/Home/Index");
            }
            else if (isCurrentLoogedInUserAdmin)
            {
                await this.usersService.Delete(userId);

                return this.Redirect("/Home/Index");
            }

            return this.View("Error");
        }

        public IActionResult UserAvatar(string imageName)
        {
            var imagePath = this.GetRootPath(this.hostingEnvironment, GlobalConstants.ImageFilesDirectory);

            if (!string.IsNullOrWhiteSpace(imageName))
            {
                var amagePathAndName = Path.Combine(imagePath, imageName);

                return this.PhysicalFile(amagePathAndName, this.BuildFileContentType(imageName));
            }

            var amagePathAndDefaultName = Path.Combine(imagePath, GlobalConstants.DefaultAvatar);

            return this.PhysicalFile(amagePathAndDefaultName, this.BuildFileContentType(GlobalConstants.DefaultAvatar));
        }
    }
}
