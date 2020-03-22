namespace DotNetInterview.Web.ViewModels.Interviews
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    using static DotNetInterview.Web.ViewModels.Constants.DataConstantVM;

    public class CreateInterviewQuestionVM
    {
        [Required(ErrorMessage = "Question content is required!")]
        [MinLength(QuestionContentMinLength, ErrorMessage = "Question content should have minimum 2 characters!")]
        [MaxLength(QuestionContentMaxLength, ErrorMessage = "Question content should have maximum 1000 characters!")]
        public string Content { get; set; }

        public IFormFile FormFile { get; set; }

        [MinLength(GivenAnswerMinLength, ErrorMessage = "Answer content should have minimum 2 characters!")]
        [MaxLength(GivenAnswerMaxLength, ErrorMessage = "Answer content should have maximum 5000 characters!")]
        public string GivenAnswer { get; set; }

        public string GivenAnswerCss { get; set; }

        public string GivenAnswerBtnText { get; set; }

        public int Interesting { get; set; }

        public int Unexpected { get; set; }

        public int Difficult { get; set; }
    }
}
