namespace DotNetInterview.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using DotNetInterview.Common;
    using DotNetInterview.Web.Controllers;
    using DotNetInterview.Web.ViewModels.Administration.Users;
    using DotNetInterview.Services.Data;
    using DotNetInterview.Web.ViewModels.Administration.Interviews;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
        private readonly IAdministrationService administratorService;

        public AdministrationController(IAdministrationService administratorService)
        {
            this.administratorService = administratorService;
        }

        [HttpGet]
        public IActionResult AdminPanel()
        {
            return this.View();
        }

        [HttpGet]
        public IActionResult DeletedInterviews()
        {
            var allDeleted = this.administratorService.GetDeletedInterviews<DeletedInterviewVM>();

            return this.View(allDeleted);
        }

        [HttpGet]
        public Task<IActionResult> UndeleteInterview(string interviewId)
        {
            return null;
        }

        [HttpGet]
        public IActionResult DetailsDeleteInterview(string interviewId)
        {
            var interview = this.administratorService.GetDetailsDeletedInterview<DetailsDeletedInterviewVM>(interviewId);
            return this.View(interview);
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

        public Task<IActionResult> AddNationality(string nationality)
        {
            return null;
        }

        [HttpGet]
        public Task<IActionResult> DeleteNationality(string nationality)
        {
            return null;
        }
    }
}
