namespace DotNetInterview.Web.ViewModels.Comments.DTO
{
    using System;

    public class AllCommentsDTO
    {
        public string CommentId { get; set; }

        public string HideDelete { get; set; }

        public string HideAdd { get; set; }

        public string ParentId { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string UserId { get; set; }

        public string UserFName { get; set; }

        public string UserLName { get; set; }
    }
}
