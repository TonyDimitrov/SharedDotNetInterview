namespace DotNetInterview.Web.ViewModels.Questions
{
    using System.Collections.Generic;

    using DotNetInterview.Web.ViewModels.Common;
    using DotNetInterview.Web.ViewModels.Interviews;

    public class AllIQuestionsVM : PaginationVM
    {
        public int Rank { get; set; }

        public string HideAddComment { get; set; }

        public int Seniority { get; set; }

        public IEnumerable<AllInterviewQuestionsVM> Questions { get; set; }
    }
}
