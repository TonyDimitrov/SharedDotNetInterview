namespace DotNetInterview.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using DotNetInterview.Data.Common.Models;

    using static DotNetInterview.Data.Common.Constants.DataConstant;

    public class Comment : BaseDeletableModel<string>
    {
        public Comment()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        [MinLength(CommentContentMinLength)]
        [MaxLength(CommentContentMaxLength)]
        public string Content { get; set; }

        public int Likes { get; set; }

        public int Dislikes { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public string InterviewId { get; set; }

        public Interview Interview { get; set; }

        public string QuestionId { get; set; }

        public Question Question { get; set; }
    }
}
