﻿namespace DotNetInterview.Web.ViewModels.Interviews.DTO
{
    using System;

    public class AllInterviewCommentsDTO
    {
        public string InterviewId { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string UserId { get; set; }

        public string UserFName { get; set; }

        public string UserLName { get; set; }
    }
}
