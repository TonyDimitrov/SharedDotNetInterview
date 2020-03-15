namespace DotNetInterview.Web.ViewModels.Questions
{
    public class AllQuestionCommentsVM
    {
        public string QuestionId { get; set; }

        public string Content { get; set; }

        public string CreatedOn { get; set; }

        public string ModifiedOn { get; set; }

        public string UserId { get; set; }

        public string UserFullName { get; set; }
    }
}
