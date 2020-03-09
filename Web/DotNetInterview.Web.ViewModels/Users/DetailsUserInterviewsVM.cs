using DotNetInterview.Web.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetInterview.Web.ViewModels.Users
{
    public class DetailsUserInterviewsVM
    {
        public string InterviewId { get; set; }

        public string Title { get; set; }

        public PositionSeniorityVM Seniority { get; set; }

        public int Qns { get; set; }

        public string Date { get; set; }

        public int Likes { get; set; }
    }
}
