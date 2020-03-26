namespace DotNetInterview.Web.ViewModels.Users
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using DotNetInterview.Web.ViewModels.Enums;

    public class DetailsUserInterviewsVM
    {
        public string InterviewId { get; set; }

        public string Title { get; set; }

        public PersonSeniorityVM Seniority { get; set; }

        public int Qns { get; set; }

        public string Date { get; set; }

        public int Likes { get; set; }
    }
}
