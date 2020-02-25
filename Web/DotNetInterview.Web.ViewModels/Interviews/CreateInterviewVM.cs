using DotNetInterview.Web.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetInterview.Web.ViewModels.Interviews
{
    public class CreateInterviewVM
    {

        public CreateInterviewVM()
        {
            this.Questions = new List<CreateInterviewQuestionVM>();
        }

        public string Seniority { get; set; }

        public string PositionTitle { get; set; }

        public string PositionDescription { get; set; }

        public string LocationType { get; set; }

        public string HeldOnInterviewLocation { get; set; }

        public string Employees { get; set; }

        public DateTime HeldOnDate { get; set; }

        public string Tags { get; set; }

        public int Likes { get; set; }

        public List<CreateInterviewQuestionVM> Questions { get; set; }

        public GetInterviewsVM Select { get; set; }
    }
}
