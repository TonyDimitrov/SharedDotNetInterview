﻿namespace DotNetInterview.Web.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using DotNetInterview.Common;
    using DotNetInterview.Services;
    using DotNetInterview.Web.ViewModels.Interviews;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    public class InterviewsController : BaseController
    {
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly IInterviewsService interviewsService;
        private readonly IImporterHelperService importerHelperService;
        private readonly IFileService fileService;

        public InterviewsController(
            IWebHostEnvironment hostingEnvironment,
            IInterviewsService interviewsService,
            IImporterHelperService importerHelperService,
            IFileService fileService)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.interviewsService = interviewsService;
            this.importerHelperService = importerHelperService;
            this.fileService = fileService;
        }

        [HttpGet]
        public IActionResult All(int seniority = 0)
        {
            var interviews = this.interviewsService.All<AllInterviewsVM>(seniority);
            return this.View(interviews);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var getCreateInterviewVM = this.interviewsService.CreateGetVM();

            getCreateInterviewVM.Select.Nationality = this.importerHelperService.GetAll<IEnumerable<string>>();

            return this.View(getCreateInterviewVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateInterviewVM model)
        {
            if (!this.ModelState.IsValid)
            {
                this.View(model);
            }

            var filePath = this.GetRootPath(this.hostingEnvironment, GlobalConstants.TaskFilesDirectory);

            await this.interviewsService.Create(model, this.GetUserId(this.User), filePath, this.fileService);

            return this.Redirect("/");
        }
    }
}
