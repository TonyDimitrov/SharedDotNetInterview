namespace DotNetInterview.Web.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    using DotNetInterview.Common;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Services.Data;
    using DotNetInterview.Services.Data.Helpers;
    using DotNetInterview.Web.ViewModels.Comments;
    using DotNetInterview.Web.ViewModels.Comments.DTO;
    using DotNetInterview.Web.ViewModels.Interviews;
    using DotNetInterview.Web.ViewModels.Interviews.DTO;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class InterviewsController : BaseController
    {
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly IInterviewsService interviewsService;
        private readonly IImporterHelperService importerHelperService;
        private readonly IFileService fileService;

        public InterviewsController(
            RoleManager<ApplicationRole> roleManager,
            IWebHostEnvironment hostingEnvironment,
            IInterviewsService interviewsService,
            IImporterHelperService importerHelperService,
            IFileService fileService)
        {
            this.roleManager = roleManager;
            this.hostingEnvironment = hostingEnvironment;
            this.interviewsService = interviewsService;
            this.importerHelperService = importerHelperService;
            this.fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> All(int seniority = 0)
        {
            var interviews = await this.interviewsService.All(seniority);
            return this.View(interviews);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var getCreateInterviewVM = this.interviewsService.CreateGetVM();

            getCreateInterviewVM.CompanyListNationalities = await this.importerHelperService.GetAll();

            return this.View(getCreateInterviewVM);
        }

        [ValidateAntiForgeryToken]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateInterviewVM model)
        {
            if (!this.ModelState.IsValid)
            {
                foreach (var q in model.Questions)
                {
                    Utils.SetStringValues<CreateInterviewQuestionVM>(q, q.GivenAnswer);
                }

                model.CompanyListNationalities = await this.importerHelperService.GetAll();

                return this.View(model);
            }

            var filePath = this.GetRootPath(this.hostingEnvironment, GlobalConstants.TaskFilesDirectory);

            await this.interviewsService.Create(model, this.GetUserId(this.User), filePath, this.fileService);

            return this.RedirectToAction("All");
        }

        [HttpGet]
        public IActionResult Details(string interviewId)
        {
            var userId = this.GetUserId(this.User);
            var isAdmin = this.User.IsInRole(GlobalConstants.AdministratorRoleName);

            var interview = this.interviewsService.Details<DetailsInterviewVM>(interviewId, userId, isAdmin);

            return this.View(interview);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(string interviewId)
        {
            var interview = await this.interviewsService.EditGet(interviewId);

            foreach (var q in interview.Questions)
            {
                Utils.SetStringValues<EditInterviewQuestionsDTO>(q, q.GivenAnswer);
            }

            interview.CompanyListNationalities = await this.importerHelperService.GetAllWithSelected(interview.CompanyNationality);

            return this.View(interview);
        }

        [ValidateAntiForgeryToken]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(EditInterviewDTO model)
        {
            var userId = this.GetUserId(this.User);

            var filePath = this.GetRootPath(this.hostingEnvironment, GlobalConstants.TaskFilesDirectory);

            await this.interviewsService.Edit(model, userId, filePath, this.fileService);

            return this.RedirectToAction("Details", new { model.InterviewId });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Delete(string interviewId)
        {
            if (string.IsNullOrWhiteSpace(interviewId))
            {
                return this.RedirectToAction("Error", "Home", "Invalid Interview ID!");
            }

            var userId = this.GetUserId(this.User);
            var isAdmin = this.IsAdmin();

            await this.interviewsService.Delete(interviewId, userId, isAdmin);

            return this.RedirectToAction("All");
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [HttpGet]
        public async Task<IActionResult> HardDelete(string interviewId)
        {
            if (string.IsNullOrWhiteSpace(interviewId))
            {
                return this.RedirectToAction("Error", "Home", "Invalid Interview ID!");
            }

            var userId = this.GetUserId(this.User);
            var isAdmin = this.IsAdmin();

            await this.interviewsService.HardDelete(interviewId, isAdmin);

            return this.RedirectToAction("All");
        }

       // [ValidateAntiForgeryToken]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody]AddCommentDTO model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var userId = this.GetUserId(this.User);

            await this.interviewsService.AddComment(model, userId);

            var isAdmin = this.IsAdmin();
            var comments = this.interviewsService.AllComments<IEnumerable<AllCommentsVM>>(model.Id, userId, isAdmin);
            var toJson = JsonSerializer.Serialize(comments.ToList()[0], new JsonSerializerOptions { WriteIndented = true });
            return this.Json(comments);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Like([FromQuery]string interviewId)
        {

            if (string.IsNullOrWhiteSpace(interviewId))
            {
                return this.BadRequest();
            }

            var userId = this.GetUserId(this.User);
            var likeVM = await this.interviewsService.Liked(interviewId, userId);

            return this.Json(likeVM);
        }
    }
}
