namespace DotNetInterview.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DotNetInterview.Services.Data;
    using DotNetInterview.Web.ViewModels.Comments;
    using DotNetInterview.Web.ViewModels.Comments.DTO;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class QuestionsController : BaseController
    {
        private readonly IQuestionsService questionsService;

        public QuestionsController(IQuestionsService questionsService)
        {
            this.questionsService = questionsService;
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
    }
}
