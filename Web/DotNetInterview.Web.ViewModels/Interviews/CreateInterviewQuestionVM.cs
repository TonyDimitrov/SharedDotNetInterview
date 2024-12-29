namespace DotNetInterview.Web.ViewModels.Interviews
{
    using DotNetInterview.Web.ViewModels.Enums;
    using Microsoft.AspNetCore.Http;

    using System.ComponentModel.DataAnnotations;

    using static DotNetInterview.Web.ViewModels.Constants.DataConstantVM;

    public class CreateInterviewQuestionVM
    {
        [Required(ErrorMessage = "Question content is required!")]
        [MinLength(QuestionContentMinLength, ErrorMessage = "Question content should have minimum 2 characters!")]
        [MaxLength(QuestionContentMaxLength, ErrorMessage = "Question content should have maximum 4000 characters!")]
        public string Content { get; set; }

        public bool IsInteresting { get; set; }
        public bool IsDifficult { get; set; }
        public bool IsUnexpected { get; set; }

        public QuestionGeneralTypeVM QuestionGeneral { get; set; }

        public IFormFile FormFile { get; set; }

        [MinLength(GivenAnswerMinLength, ErrorMessage = "Answer content should have minimum 2 characters!")]
        [MaxLength(GivenAnswerMaxLength, ErrorMessage = "Answer content should have maximum 6000 characters!")]
        public string GivenAnswer { get; set; }

        public string GivenAnswerCss { get; set; }

        public string GivenAnswerBtnText { get; set; }

        public int Interesting { get; set; }

        public int Unexpected { get; set; }

        public int Difficult { get; set; }
    }
}
