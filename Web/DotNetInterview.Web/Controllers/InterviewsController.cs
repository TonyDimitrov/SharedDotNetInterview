namespace DotNetInterview.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;

    using DotNetInterview.Common;
    using DotNetInterview.Services.Data;
    using DotNetInterview.Services.Data.Helpers;
    using DotNetInterview.Web.Infrastructure.Extensions;
    using DotNetInterview.Web.ViewModels;
    using DotNetInterview.Web.ViewModels.Comments;
    using DotNetInterview.Web.ViewModels.Comments.DTO;
    using DotNetInterview.Web.ViewModels.Interviews;
    using DotNetInterview.Web.ViewModels.Interviews.DTO;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.AspNetCore.Mvc;

    public class InterviewsController : BaseController
    {
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly IInterviewsService interviewsService;
        private readonly INationalitiesService nationalitiesService;
        private readonly IFileService fileService;

        public InterviewsController(
            IWebHostEnvironment hostingEnvironment,
            IInterviewsService interviewsService,
            INationalitiesService nationalitiesService,
            IFileService fileService)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.interviewsService = interviewsService;
            this.nationalitiesService = nationalitiesService;
            this.fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> All([FromQuery] AllAjaxInterviewDTO model)
        {
            var interviewsVM = await this.interviewsService.All(model.Seniority);

            var nationalities = interviewsVM.Nationalities.InsertCommonElementInList("All");

            interviewsVM.Nationalities = nationalities;

            var interviewsByPage = this.interviewsService.AllByPage(model.Page.HasValue ? model.Page.Value : 0, interviewsVM, interviewsVM.Interviews);

            return this.View(interviewsByPage);
        }

        [HttpGet]
        public async Task<IActionResult> AllAjax(AllAjaxInterviewDTO model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("All", new { model.Seniority, model.Page });
            }

            var interviewsVM = await this.interviewsService.AllByFilter(model);

            var nationalities = interviewsVM.Nationalities.InsertCommonElementInList("All");

            interviewsVM.Nationalities = nationalities;

            if (model.Page.HasValue)
            {
                interviewsVM = this.interviewsService.AllByPage(model.Page.Value, interviewsVM, interviewsVM.Interviews);
            }

            return this.Json(interviewsVM);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var getCreateInterviewVM = this.interviewsService.CreateGetVM();

            getCreateInterviewVM.CompanyListNationalities = await this.nationalitiesService.GetAll();
            getCreateInterviewVM.HodlOnDate = DateTime.Now;

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

                model.CompanyListNationalities = await this.nationalitiesService.GetAll();

                return this.View(model);
            }

            var filePath = this.GetRootPath(this.hostingEnvironment, GlobalConstants.TaskFilesDirectory);

            await this.interviewsService.Create(model, this.GetLoggedInUserId(this.User), filePath, this.fileService);

            return this.RedirectToAction("All");
        }

        [HttpGet]
        public IActionResult Details(string interviewId)
        {
            var userId = this.GetLoggedInUserId(this.User);
            var isAdmin = this.User.IsInRole(GlobalConstants.AdministratorRoleName);

            var interview = this.interviewsService.Details(interviewId, userId, isAdmin);

            if (interview == null)
            {
                var errorVM = new ItemNotFoundErrorVM
                {
                    ItemId = interviewId,
                    Message = string.Format(ErrorMessages.ItemNotFound, "Interview", interviewId),
                    RequestUrl = this.HttpContext.Request.GetDisplayUrl(),
                };

                return this.RedirectToAction("ItemNotFound", "NotFound", errorVM);
            }

            return this.View(interview);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(string interviewId)
        {
            var interview = await this.interviewsService.EditGet(interviewId);

            if (interview == null)
            {
                var errorVM = new ItemNotFoundErrorVM
                {
                    ItemId = interviewId,
                    Message = string.Format(ErrorMessages.ItemNotFound, "Interview", interviewId),
                    RequestUrl = this.HttpContext.Request.GetDisplayUrl(),
                };

                return this.RedirectToAction("ItemNotFound", "NotFound", errorVM);
            }

            foreach (var q in interview.Questions)
            {
                Utils.SetStringValues<EditInterviewQuestionsDTO>(q, q.GivenAnswer);
            }

            return this.View(interview);
        }

        [ValidateAntiForgeryToken]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(EditInterviewDTO model)
        {
            var userId = this.GetLoggedInUserId(this.User);

            var filePath = this.GetRootPath(this.hostingEnvironment, GlobalConstants.TaskFilesDirectory);

            await this.interviewsService.Edit(model, userId, filePath, this.fileService);

            return this.RedirectToAction("Details", new { model.InterviewId });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Delete(string interviewId)
        {
            var userId = this.GetLoggedInUserId(this.User);
            var isAdmin = this.IsAdmin();

            await this.interviewsService.Delete(interviewId, userId, isAdmin);

            return this.RedirectToAction("All");
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [HttpGet]
        public async Task<IActionResult> HardDelete(string interviewId)
        {
            var userId = this.GetLoggedInUserId(this.User);
            var isAdmin = this.IsAdmin();

            await this.interviewsService.HardDelete(interviewId, isAdmin);

            return this.RedirectToAction("All");
        }

        [ValidateAntiForgeryToken]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] AddCommentDTO model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var userId = this.GetLoggedInUserId(this.User);
            var isAdmin = this.IsAdmin();

            try
            {
                await this.interviewsService.AddComment(model, userId);

                var comments = this.interviewsService.AllComments<IEnumerable<AllCommentsVM>>(model.Id, userId, isAdmin);
                var toJson = JsonSerializer.Serialize(comments.ToList()[0], new JsonSerializerOptions { WriteIndented = true });

                return this.Json(comments);
            }
            catch (Exception)
            {
                return this.View(GlobalConstants.ErrorView, "An error accure while adding comment!");
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Like([FromQuery] string interviewId)
        {
            if (string.IsNullOrWhiteSpace(interviewId))
            {
                return this.BadRequest();
            }

            var userId = this.GetLoggedInUserId(this.User);
            var likeVM = await this.interviewsService.Liked(interviewId, userId);

            return this.Json(likeVM);
        }
    }
}
