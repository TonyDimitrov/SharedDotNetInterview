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

        [Required(ErrorMessage = "Cannot leave given answer empty!")]
        [MinLength(GivenAnswerMinLength, ErrorMessage = "Question content should have minimum 2 characters!")]
        [MaxLength(GivenAnswerMaxLength, ErrorMessage = "Question content should have maximum 5000 characters!")]
        public string GivenAnswer { get; set; }

        public int Interesting { get; set; }

        public int Unexpected { get; set; }

        public int Difficult { get; set; }
    }
}
