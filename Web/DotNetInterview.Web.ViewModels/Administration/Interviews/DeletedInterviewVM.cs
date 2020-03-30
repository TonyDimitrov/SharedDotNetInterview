using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetInterview.Web.ViewModels.Administration.Interviews
{
    public class DeletedInterviewVM
    {
        public string InterviewId { get; set; }

        public string PositionTitle { get; set; }

        public int Questions { get; set; }

        public string CreatedOn { get; set; }

        public string DeletedOn { get; set; }

        public string Seniority { get; set; }

        public string CreatorId { get; set; }

        public string CreatorName { get; set; }

        public string CreatorAvatar { get; set; }
    }
}
