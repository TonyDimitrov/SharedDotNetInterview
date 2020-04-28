namespace DotNetInterview.Web.Controllers
{
    using System.Threading.Tasks;

    using DotNetInterview.Common;
    using DotNetInterview.Services.Data;
    using DotNetInterview.Web.PdfGenerator;
    using DotNetInterview.Web.ViewModels;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.AspNetCore.Mvc;

    public class PdfController : BaseController
    {
        private readonly IViewRenderService viewRenderService;
        private readonly IHtmlToPdfConverter htmlToPdfConverter;
        private readonly IWebHostEnvironment environment;
        private readonly IInterviewsService interviewsService;

        public PdfController(
            IViewRenderService viewRenderService,
            IHtmlToPdfConverter htmlToPdfConverter,
            IWebHostEnvironment environment,
            IInterviewsService interviewsService)
        {
            this.viewRenderService = viewRenderService;
            this.htmlToPdfConverter = htmlToPdfConverter;
            this.environment = environment;
            this.interviewsService = interviewsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPdf(string interviewId)
        {
            var interview = this.interviewsService.Details(interviewId, "not existing", false);

            if (interview == null)
            {
                var errorVM = new ItemNotFoundErrorVM
                {
                    ItemId = interviewId,
                    Message = string.Format(ErrorMessages.ItemNotFound, "Interview", interviewId),
                    RequestUrl = this.HttpContext.Request.GetDisplayUrl(),
                };

                return this.RedirectToAction("ItemNotFound", "NotFound", errorVM);
            }

            var htmlData = await this.viewRenderService.RenderToStringAsync("~/Views/Interviews/Details.cshtml", interview);

            var pathToJsFile = this.environment.WebRootPath + "\\temp-pdf";

            var fileContents = this.htmlToPdfConverter.Convert(pathToJsFile,  htmlData, FormatType.A4, OrientationType.Portrait);
            return this.File(fileContents, "application/pdf");
        }
    }
}
