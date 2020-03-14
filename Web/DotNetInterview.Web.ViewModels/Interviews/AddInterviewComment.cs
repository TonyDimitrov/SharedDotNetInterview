using System.ComponentModel.DataAnnotations;

namespace DotNetInterview.Web.ViewModels.Interviews
{
    public class AddInterviewComment
    {
        [Required]
        [MinLength(5)]
        public string InterviewId { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(400)]
        public string Content { get; set; }
    }
}
