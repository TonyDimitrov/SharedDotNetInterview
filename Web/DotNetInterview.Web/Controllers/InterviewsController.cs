namespace DotNetInterview.Web.Controllers
{
    using System.IO;
    using System.Threading.Tasks;

    using DotNetInterview.Services;
    using DotNetInterview.Web.ViewModels.Interviews;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    public class InterviewsController : BaseController
    {
        private const string TaskFilesDirectory = "uploads\\taskFiles\\";
        private const string ImageFilesDirectory = "uploads\\imageFiles\\";

        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly IInterviewsService interviewsService;

        public InterviewsController(IWebHostEnvironment hostingEnvironment, IInterviewsService interviewsService)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.interviewsService = interviewsService;
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

            return this.View(getCreateInterviewVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateInterviewVM model)
        {
            if (!this.ModelState.IsValid)
            {
                this.View(model);
            }

            var filePath = this.GetRootPath(TaskFilesDirectory);

            await this.interviewsService.Create(model, this.GetUserId(this.User), filePath);

            return this.Redirect("/");
        }

        private string GetRootPath(string typeFilesDirectory)
        {
            return Path.Combine(this.hostingEnvironment.WebRootPath, typeFilesDirectory);
        }
    }
}
