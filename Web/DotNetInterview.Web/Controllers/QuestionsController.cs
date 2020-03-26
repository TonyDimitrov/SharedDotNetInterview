namespace DotNetInterview.Web.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using DotNetInterview.Common;
    using DotNetInterview.Services.Data;
    using DotNetInterview.Web.ViewModels.Comments;
    using DotNetInterview.Web.ViewModels.Comments.DTO;
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody]AddCommentDTO model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var userId = this.GetUserId(this.User);

            await this.questionsService.AddComment(model, userId);

            var isAdmin = this.IsAdmin();
            var comments = this.questionsService.AllComments<IEnumerable<AllCommentsVM>>(model.Id, userId, isAdmin);

            return this.Json(comments);
        }

        [HttpGet]
        public IActionResult All(int ranked)
        {
            var userId = this.GetUserId(this.User);
            var isAdmin = this.IsAdmin();
            var questions = this.questionsService.All(ranked, userId, isAdmin);

            return this.View(questions);
        }

        [HttpGet]
        public IActionResult File(string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var imagePath = this.GetRootPath(this.hostingEnvironment, GlobalConstants.TaskFilesDirectory);

                var filePathAndName = Path.Combine(imagePath, fileName);

                var fileExtension = this.FileExtension(fileName);

                return this.PhysicalFile(filePathAndName, this.BuildFileContenttype(fileName), "file." + fileExtension);
            }

            return null;
        }

        [HttpGet]
        public IActionResult RankImage()
        {
            var imagePath = this.GetRootPath(this.hostingEnvironment, GlobalConstants.ImageFilesDirectory);

            var imageName = GlobalConstants.DefaultRanking;

            var filePathAndName = Path.Combine(imagePath, imageName);

            var fileExtension = this.FileExtension(imageName);

            return this.PhysicalFile(filePathAndName, this.BuildFileContenttype(imageName));
        }
    }
}
