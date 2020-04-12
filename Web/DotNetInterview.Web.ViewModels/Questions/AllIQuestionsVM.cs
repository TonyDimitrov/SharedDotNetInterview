namespace DotNetInterview.Web.ViewModels.Questions
{
    using System.Collections.Generic;

    using DotNetInterview.Web.ViewModels.Common;
    using DotNetInterview.Web.ViewModels.Interviews;

    public class AllIQuestionsVM : PaginationVM
    {
        public AllIQuestionsVM(int rank, string hideAddComment)
        {
            this.Rank = rank;
            this.HideAddComment = hideAddComment;
        }

        public int Rank { get; private set; }

        public string HideAddComment { get; private set; }

        public IEnumerable<AllInterviewQuestionsVM> Questions { get; set; }
    }
}
