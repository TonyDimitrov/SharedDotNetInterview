namespace DotNetInterview.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DotNetInterview.Services.Data;
    using DotNetInterview.Web.ViewModels.Comments;
    using DotNetInterview.Web.ViewModels.Comments.DTO;
    using Microsoft.AspNetCore.Mvc;

    public class QuestionsController : BaseController
    {
        private readonly IQuestionsService questionsService;

        public QuestionsController(IQuestionsService questionsService)
        {
            this.questionsService = questionsService;
        }

        public async Task<IActionResult> AddComment([FromBody]AddCommentDTO model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var userId = this.GetUserId(this.User);

            await this.questionsService.AddComment(model, userId);

            var comments = this.questionsService.AllComments<IEnumerable<AllCommentsVM>>(model.Id);

            return this.Json(comments);
        }
    }
}
