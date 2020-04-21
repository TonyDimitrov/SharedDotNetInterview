namespace DotNetInterview.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using DotNetInterview.Common;
    using DotNetInterview.Services.Data;
    using DotNetInterview.Web.Controllers;
    using DotNetInterview.Web.ViewModels;
    using DotNetInterview.Web.ViewModels.Administration.Interviews;
    using DotNetInterview.Web.ViewModels.Administration.Nationalities;
    using DotNetInterview.Web.ViewModels.Administration.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.AspNetCore.Mvc;

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
        public IActionResult DeletedUsers(int page = 1)
        {
            var allDeleted = this.administratorService.GetAllDeletedUsers<DeletedUserVM>();

            var usersPerPage = this.administratorService.GetDeletedUsersByPage(page, new DeletedUsersVM(), allDeleted);

            return this.View(usersPerPage);
        }

        [HttpGet]
        public IActionResult DetailsDeletedUser(string userId)
        {
            var deletedUser = this.administratorService.GetDetailsDeletedUser<DetailsDeletedUser>(userId);

            if (deletedUser == null)
            {
                return this.RedirectToAction("ItemNotFound", "NotFound", new { area = string.Empty });
            }

            return this.View(deletedUser);
        }

        [HttpGet]
        public async Task<IActionResult> UndeleteUser(string userId)
        {
            await this.administratorService.UndeleteUser(userId);

            return this.RedirectToAction("DeletedUsers");
        }

        [HttpGet]
        public IActionResult DeletedInterviews(int page = 1)
        {
            var allDeleted = this.administratorService.GetDeletedInterviews<DeletedInterviewVM>();

            var interviewsPerPage = this.administratorService.GetDeletedInterviewsByPage(page, new DeletedInterviewsVM(), allDeleted);

            return this.View(interviewsPerPage);
        }

        [HttpGet]
        public IActionResult DetailsDeletedInterview(string interviewId)
        {
            var interview = this.administratorService.GetDetailsDeletedInterview<DetailsDeletedInterviewVM>(interviewId);

            if (interview == null)
            {
                return this.RedirectToAction("ItemNotFound", "NotFound", new { area = string.Empty });
            }

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
        public async Task<IActionResult> AddNationality(ManageNationalitiesVM model)
        {
            if (!this.ModelState.IsValid)
            {
                model.Nationalities = await this.importerHelperService.GetAll();

                return this.View(nameof(this.ManageNationalitiesGet), model);
            }

            var added = await this.importerHelperService.AddNationality(model.Add);

            model.StatusMessage = added.Message;
            model.Nationalities = await this.importerHelperService.GetAll();

            return this.View(nameof(this.ManageNationalitiesGet), model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNationality(ManageNationalitiesVM model)
        {
            if (!this.ModelState.IsValid)
            {
                model.Nationalities = await this.importerHelperService.GetAll();

                return this.View(nameof(this.ManageNationalitiesGet), model);
            }

            var deleted = await this.importerHelperService.DeleteNationality(model.Delete);

            model.StatusMessage = deleted.Message;
            model.Nationalities = await this.importerHelperService.GetAllWithSelected(model.Delete);

            return this.View(nameof(this.ManageNationalitiesGet), model);
        }
    }
}
