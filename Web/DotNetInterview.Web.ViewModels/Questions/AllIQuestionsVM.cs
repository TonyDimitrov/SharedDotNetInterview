namespace DotNetInterview.Web.ViewModels.Questions
{
    using System.Collections.Generic;

    using DotNetInterview.Web.ViewModels.Interviews;

    public class AllIQuestionsVM
    {
        public AllIQuestionsVM()
        {
            this.Questions = new List<AllInterviewQuestionsVM>();
        }

        public int Rank { get; set; }

        public string HideAddComment { get; set; }

        public int Seniority { get; set; }

        public int PaginationLength { get; set; } = 3;

        public int StartrIndex { get; set; }

        public int CurrentSet { get; set; }

        public string PrevtDisable { get; set; }

        public string NextDisable { get; set; }

        public IEnumerable<AllInterviewQuestionsVM> Questions { get; set; }
    }
}
