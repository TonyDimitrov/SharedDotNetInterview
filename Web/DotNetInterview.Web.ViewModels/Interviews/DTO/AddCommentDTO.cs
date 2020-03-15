namespace DotNetInterview.Web.ViewModels.Interviews.DTO
{
    using System.ComponentModel.DataAnnotations;

    public class AddCommentDTO
    {
        [Required]
        [MinLength(5)]
        public string Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(400)]
        public string Content { get; set; }
    }
}
