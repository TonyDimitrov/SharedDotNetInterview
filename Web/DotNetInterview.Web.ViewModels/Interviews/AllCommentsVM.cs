namespace DotNetInterview.Web.ViewModels.Comments
{
    public class AllCommentsVM
    {
        public string CommentId { get; set; }

        public bool CanDelete { get; set; }

        public bool CanAdd { get; set; }

        public string DefaultValue { get; set; }

        public string ParentId { get; set; }

        public string Content { get; set; }

        public string CreatedOn { get; set; }

        public string ModifiedOn { get; set; }

        public bool HasBeenModified { get; set; }

        public string UserId { get; set; }

        public string UserFullName { get; set; }
    }
}
