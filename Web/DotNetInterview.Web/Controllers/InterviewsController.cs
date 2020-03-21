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
    using DotNetInterview.Web.ViewModels.Comments;
    using DotNetInterview.Web.ViewModels.Comments.DTO;
    using DotNetInterview.Web.ViewModels.Interviews;
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
        public IActionResult All(int seniority = 0)
        {
            var interviews = this.interviewsService.All<AllInterviewsVM>(seniority);
            return this.View(interviews);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var getCreateInterviewVM = this.interviewsService.CreateGetVM();

            getCreateInterviewVM.Select.Nationality = this.importerHelperService.GetAll();

            return this.View(getCreateInterviewVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateInterviewVM model)
        {
            if (!this.ModelState.IsValid)
            {
                this.View(model);
            }

            var filePath = this.GetRootPath(this.hostingEnvironment, GlobalConstants.TaskFilesDirectory);

            await this.interviewsService.Create(model, this.GetUserId(this.User), filePath, this.fileService);

            return this.Redirect("/");
        }

        [HttpGet]
        public IActionResult Details(string interviewId)
        {
            var userId = this.GetUserId(this.User);
            var isAdmin = this.User.IsInRole(GlobalConstants.AdministratorRoleName);

            var interview = this.interviewsService.Details<DetailsInterviewVM>(interviewId, userId, isAdmin);

            return this.View(interview);
        }

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
