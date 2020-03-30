namespace DotNetInterview.Web.Areas.Administration.Controllers
{
    using DotNetInterview.Common;
    using DotNetInterview.Web.Controllers;
    using DotNetInterview.Web.ViewModels.Administration.Interviews;
    using DotNetInterview.Web.ViewModels.Administration.Nationalities;
    using DotNetInterview.Web.ViewModels.Administration.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
        [HttpGet]
        public IActionResult AdminPanel()
        {
            return this.View();
        }

        [HttpGet]
        public IActionResult DeletedInterviews()
        {
            return this.View(new DeletedInterviewsVM());
        }

        [HttpGet]
        public IActionResult DeletedUsers()
        {
            return this.View(new DeletedUsersVM());
        }

        [HttpGet]
        public IActionResult ManageNationalitiesGet()
        {
            return this.View();
        }

        [HttpGet]
        public IActionResult ManageNationalities(ManageNationalitiesVM model)
        {
            return this.View();
        }
    }
}
