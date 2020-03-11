namespace DotNetInterview.Web.ViewModels.Interviews.DTO
{
    using System;

    public class AllInterviewCommentsDTO
    {
        public string InterviewId { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
