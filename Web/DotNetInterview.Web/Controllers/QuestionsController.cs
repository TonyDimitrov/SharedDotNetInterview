namespace DotNetInterview.Web.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using DotNetInterview.Common;
    using DotNetInterview.Services.Data;
    using DotNetInterview.Web.ViewModels.Comments;
    using DotNetInterview.Web.ViewModels.Comments.DTO;
    using DotNetInterview.Web.ViewModels.Questions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    public class QuestionsController : BaseController
    {
        private readonly IQuestionsService questionsService;
        private readonly IWebHostEnvironment hostingEnvironment;

        public QuestionsController(IQuestionsService questionsService, IWebHostEnvironment hostingEnvironment)
        {
            this.questionsService = questionsService;
            this.hostingEnvironment = hostingEnvironment;
        }

        [ValidateAntiForgeryToken]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody]AddCommentDTO model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var userId = this.GetLoggedInUserId(this.User);

            await this.questionsService.AddComment(model, userId);

            var isAdmin = this.IsAdmin();
            var comments = this.questionsService.AllComments<IEnumerable<AllCommentsVM>>(model.Id, userId, isAdmin);

            return this.Json(comments);
        }

        [HttpGet]
        public IActionResult All([FromQuery]int ranked, int page = 1)
        {
            var userId = this.GetLoggedInUserId(this.User);
            var isAdmin = this.IsAdmin();

            var questions = this.questionsService.All(ranked, userId, isAdmin);

            var questionsByPage = this.questionsService.AllByPage(
                page,
                new AllIQuestionsVM(questions.Rank, questions.HideAddComment),
                questions.Questions);

            return this.View(questionsByPage);
        }

        [HttpGet]
        public IActionResult File(string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var imagePath = this.GetRootPath(this.hostingEnvironment, GlobalConstants.TaskFilesDirectory);

                var filePathAndName = Path.Combine(imagePath, fileName);

                var fileExtension = this.FileExtension(fileName);

                return this.PhysicalFile(filePathAndName, this.BuildFileContentType(fileName), "file." + fileExtension);
            }

            return null;
        }

        [HttpGet]
        public IActionResult RankImage()
        {
            var imagePath = this.GetRootPath(this.hostingEnvironment, GlobalConstants.ImageFilesDirectory);

            var imageName = GlobalConstants.DefaultRanking;

            var filePathAndName = Path.Combine(imagePath, imageName);

            return this.PhysicalFile(filePathAndName, this.BuildFileContentType(imageName));
        }
    }
}
