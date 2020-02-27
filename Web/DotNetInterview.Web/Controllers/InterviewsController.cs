namespace DotNetInterview.Web.Controllers
{
    using System.Collections.Generic;
    using System.IO;

    using DotNetInterview.Services;
    using DotNetInterview.Web.ViewModels.Interviews;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class InterviewsController : BaseController
    {
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly IInterviewsService interviewsService;

        public InterviewsController(IWebHostEnvironment hostingEnvironment, IInterviewsService interviewsService)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.interviewsService = interviewsService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var createGetData = new GetCreateInterviewsVM
            {
                Nationality = new List<string> { "Bulgaria", "UK", "USA", "France" },
            };
            var list = new List<CreateInterviewQuestionVM>
            {
                new CreateInterviewQuestionVM(),
                new CreateInterviewQuestionVM(),
            };

            return this.View(new CreateInterviewVM { Select = createGetData, Questions = list });
        }

        [HttpPost]
        public IActionResult Create(CreateInterviewVM model)
        {
            var fileObject = model.Questions[0].FormFile;

            if (fileObject != null)
            {
                var uniqueFileName = fileObject.FileName;
                var uploads = Path.Combine(this.hostingEnvironment.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    fileObject.CopyTo(stream);
                }
            }

            return this.Redirect("/");
        }

        private byte[] GetByteArrayFromImage(IFormFile file)
        {
            using (var target = new MemoryStream())
            {
                file.CopyTo(target);
                return target.ToArray();
            }
        }
    }
}
