namespace DotNetInterview.Web.ViewModels.Interviews
{
    using System.Collections.Generic;

    using DotNetInterview.Web.ViewModels.Questions;

    public class AllInterviewQuestionsVM
    {
        public string QuestionId { get; set; }

        public string Content { get; set; }

        public string Answer { get; set; }

        public int Ranked { get; set; }

        public string CreatedOn { get; set; }

        public string ModifiedOn { get; set; }

        public string File { get; set; }

        public IEnumerable<AllQuestionCommentsVM> QnsComments { get; set; }
    }
}
