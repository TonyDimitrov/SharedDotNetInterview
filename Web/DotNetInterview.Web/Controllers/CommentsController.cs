namespace DotNetInterview.Web.Controllers
{
    using System.Threading.Tasks;

    using DotNetInterview.Common;
    using DotNetInterview.Services.Data;
    using DotNetInterview.Web.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.AspNetCore.Mvc;

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

            if (!deleted)
            {
                var errorVM = new ItemNotFoundErrorVM
                {
                    ItemId = id,
                    Message = string.Format(ErrorMessages.ItemNotFound, "Interview", id),
                    RequestUrl = this.HttpContext.Request.GetDisplayUrl(),
                };

                return this.RedirectToAction("ItemNotFound", "NotFound", errorVM);
            }

            return this.Json("Deleted");
        }
    }
}
