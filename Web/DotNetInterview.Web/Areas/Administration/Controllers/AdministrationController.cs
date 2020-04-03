﻿namespace DotNetInterview.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using DotNetInterview.Common;
    using DotNetInterview.Web.Controllers;
    using DotNetInterview.Web.ViewModels.Administration.Users;
    using DotNetInterview.Services.Data;
    using DotNetInterview.Web.ViewModels.Administration.Interviews;
    using DotNetInterview.Web.ViewModels.Administration.Nationalities;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
        private readonly IAdministrationService administratorService;
        private readonly IImporterHelperService importerHelperService;

        public AdministrationController(
            IAdministrationService administratorService,
            IImporterHelperService importerHelperService)
        {
            this.administratorService = administratorService;
            this.importerHelperService = importerHelperService;
        }

        [HttpGet]
        public IActionResult AdminPanel()
        {
            return this.View();
        }

        [HttpGet]
        public IActionResult DeletedUsers(int? page)
        {
            var allDeleted = this.administratorService.GetAllDeletedUsers<DeletedUserVM>();

            return this.View(new DeletedUsersVM { DeletedUsers = allDeleted });
        }

        [HttpGet]
        public IActionResult DetailsDeletedUser(string userId)
        {
            var deletedUser = this.administratorService.GetDetailsDeletedUser<DetailsDeletedUser>(userId);

            return this.View(deletedUser);
        }

        [HttpGet]
        public async Task<IActionResult> UndeleteUser(string userId)
        {
           await this.administratorService.UndeleteUser(userId);

           return this.RedirectToAction("DeletedUsers");
        }

        public IActionResult DeletedInterviews()
        {
            var allDeleted = this.administratorService.GetDeletedInterviews<DeletedInterviewVM>();

            return this.View(allDeleted);
        }

        [HttpGet]
        public IActionResult DetailsDeletedInterview(string interviewId)
        {
            var interview = this.administratorService.GetDetailsDeletedInterview<DetailsDeletedInterviewVM>(interviewId);

            return this.View(interview);
        }

        [HttpGet]
        public async Task<IActionResult> UndeleteInterview(string interviewId)
        {
           await this.administratorService.UndeleteInterview(interviewId);

           return this.RedirectToAction("DeletedInterviews");
        }

        [HttpGet]
        public async Task<IActionResult> ManageNationalitiesGet()
        {
            var selectListItems = await this.importerHelperService.GetAll();

            return this.View(new ManageNationalitiesVM { Nationalities = selectListItems });
        }

        [HttpPost]
        public Task<IActionResult> AddNationality(string nationality)
        {
            return null;
        }

        [HttpPost]
        public Task<IActionResult> DeleteNationality(string nationality)
        {
            return null;
        }
    }
}
