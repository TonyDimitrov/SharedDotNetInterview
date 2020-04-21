namespace DotNetInterview.Web.Controllers
{
    using System.Diagnostics;

    using DotNetInterview.Web.ViewModels;
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.AspNetCore.Mvc;

    public class NotFoundController : BaseController
    {
        [Route("NotFound/{statusCode}")]
        public IActionResult ResourceNotFound([FromRoute]int statusCode)
        {
            if (statusCode == 404)
            {
                var errorVM = new NotFoundErrorVM
                {
                    RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
                    Message = "Resource that you are trying to reach was not found!",
                    StatusCode = statusCode,
                    RequestUrl = this.Request.GetDisplayUrl(),
                };
                return this.View(errorVM);
            }
            else if (statusCode >= 500 && statusCode <= 599)
            {
                var errorVM = new ErrorVM
                {
                    RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
                };

                this.View("/Error", errorVM);
            }

            return this.Redirect("Home/Index");
        }

        public IActionResult ItemNotFound(ItemNotFoundErrorVM model)
        {
            return this.View(model);
        }
    }
}
