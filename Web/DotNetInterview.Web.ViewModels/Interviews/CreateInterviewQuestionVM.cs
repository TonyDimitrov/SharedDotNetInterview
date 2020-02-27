namespace DotNetInterview.Web.ViewModels.Interviews
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    using static DotNetInterview.Web.ViewModels.Constants.DataConstantVM;

    public class CreateInterviewQuestionVM
    {
        [Required]
        [MinLength(QuestionContentMinLength)]
        [MaxLength(QuestionContentMaxLength)]
        public string Content { get; set; }

        public IFormFile FormFile { get; set; }

        [MinLength(GivenAnswerMinLength)]
        [MaxLength(GivenAnswerMaxLength)]
        public string GivenAnswer { get; set; }

        public int Interesting { get; set; }

        public int Unexpected { get; set; }

        public int Difficult { get; set; }
    }
}
