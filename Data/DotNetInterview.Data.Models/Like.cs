namespace DotNetInterview.Data.Models
{
    using System;

    using DotNetInterview.Data.Common.Models;

    public class Like : BaseDeletableModel<string>
    {
        public Like()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public int Count { get; set; }

        public bool IsLiked { get; set; }

        public int Likes { get; set; }

        public string InterviewId { get; set; }

        public virtual Interview Interview { get; set; }

        public string QuestionId { get; set; }

        public Question Question { get; set; }

        public string AnswerId { get; set; }

        public Answer Answer { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
