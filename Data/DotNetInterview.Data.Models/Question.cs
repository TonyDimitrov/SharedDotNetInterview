namespace DotNetInterview.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using DotNetInterview.Data.Common.Models;
    using DotNetInterview.Data.Models.Enums;

    using static DotNetInterview.Data.Common.Constants.DataConstant;

    public class Question : BaseDeletableModel<string>
    {
        public Question()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Answers = new HashSet<Answer>();
            this.Comments = new HashSet<Comment>();
            this.Likes = new HashSet<Like>();
        }

        [Required]
        [MinLength(QuestionContentMinLength)]
        [MaxLength(QuestionContentMaxLength)]
        public string Content { get; set; }

        public byte[] FileTask { get; set; }

        [MinLength(UrlMinLength)]
        [MaxLength(UrlMaxLength)]
        public string UrlTask { get; set; }

        [MinLength(UrlMinLength)]
        [MaxLength(UrlMaxLength)]
        public string UrlGitRepo { get; set; }

        [MinLength(GivenAnswerMinLength)]
        [MaxLength(GivenAnswerMaxLength)]
        public string GivenAnswer { get; set; }

        public QuestionRankType RankType { get; set; }

        public string InterviewId { get; set; }

        public Interview Interview { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Like> Likes { get; set; }
    }
}
