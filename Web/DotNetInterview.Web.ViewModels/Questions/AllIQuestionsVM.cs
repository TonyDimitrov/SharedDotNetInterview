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

        public string HideAddComment { get; set; }

        public IEnumerable<AllInterviewQuestionsVM> Questions { get; set; }
    }
}
