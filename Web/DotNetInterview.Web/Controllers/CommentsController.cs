namespace DotNetInterview.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using DotNetInterview.Services.Data;

    [Authorize]
    public class CommentsController : BaseController
    {
        private readonly ICommentsService commentsService;

        public CommentsController(ICommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody]string id)
        {
            var currentUserId = this.GetLoggedInUserId(this.User);
            var isAdmin = this.IsAdmin();

            var deleted = await this.commentsService.Delete(id, currentUserId, isAdmin);

            if (deleted)
            {
                return this.Json("Deleted");
            }

            return this.NotFound();
        }
    }
}
