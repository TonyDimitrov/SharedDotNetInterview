namespace DotNetInterview.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using DotNetInterview.Data.Common.Models;

    using static DotNetInterview.Data.Common.Constants.DataConstant;

    public class Answer : BaseDeletableModel<string>
    {
        public Answer()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Comments = new List<Comment>();
            this.Likes = new List<Like>();
        }

        [MinLength(AnswerContentMinLength)]
        [MaxLength(AnswerContentMaxLength)]
        public string Content { get; set; }

        public string QuestionId { get; set; }

        public Question Question { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public IEnumerable<Comment> Comments { get; set; }

        public IEnumerable<Like> Likes { get; set; }
    }
}
