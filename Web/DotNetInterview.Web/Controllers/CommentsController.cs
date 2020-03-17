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

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody]string id)
        {
            var deleted = await this.commentsService.Delete(id);

            if (deleted)
            {
                return this.Json("Toni");
            }

            return this.NotFound();
        }
    }
}
