namespace DotNetInterview.Web.ViewModels.Interviews.DTO
{
    using System;
    using System.Collections.Generic;

    using DotNetInterview.Web.ViewModels.Comments.DTO;
    using DotNetInterview.Web.ViewModels.Enums;

    public class AllInterviewQuestionsDTO
    {
        public string QuestionId { get; set; }

        public string Content { get; set; }

        public string Answer { get; set; }

        public QuestionRankTypeVM Ranked { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string File { get; set; }

        public IEnumerable<AllCommentsDTO> QnsComments { get; set; }
    }
}
