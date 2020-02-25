using DotNetInterview.Web.ViewModels.Interviews;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetInterview.Web.Controllers
{
    public class InterviewsController : BaseController
    {

        [HttpGet]
        public IActionResult Create()
        {
            var createGetData = new GetInterviewsVM
            {
                Nationality = new List<string> { "Bulgaria", "UK", "USA", "France" },
                LocationType = new List<string> { "In Office", "Remote" },
                RankQuestion = new List<string> { "Most interesting", "Most difficult", "Most unexpected" },
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
            var test = model;
            return this.Redirect("/");
        }


    }
}
