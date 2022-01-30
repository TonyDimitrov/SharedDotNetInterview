namespace DotNetInterview.Web.ViewModels.Interviews
{
    using System.Collections.Generic;

    using DotNetInterview.Web.ViewModels.Comments;
    using DotNetInterview.Web.ViewModels.Enums;

    public class AllInterviewQuestionsVM
    {
        public string QuestionId { get; set; }

        public string Content { get; set; }

        public bool HideAnswer { get; set; }

        public string Answer { get; set; }

        public bool HideRanked { get; set; }

        public string Ranked { get; set; }

        public string RankImgName { get; set; }

        public string CreatedOn { get; set; }

        public string ModifiedOn { get; set; }

        public bool HideFile { get; set; }

        public string File { get; set; }

        public int Answers { get; set; }

        public string InitiallyAnsweredCss { get; set; }

        public string InterviewId { get; set; }

        public string UserId { get; set; }

        public string UserFullName { get; set; }

        public IEnumerable<AllCommentsVM> QnsComments { get; set; }
    }
}
