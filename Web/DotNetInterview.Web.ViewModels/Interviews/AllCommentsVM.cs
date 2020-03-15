namespace DotNetInterview.Web.ViewModels.Interviews
{
    public class AllCommentsVM
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public string CreatedOn { get; set; }

        public string ModifiedOn { get; set; }

        public bool HasBeenModified { get; set; }

        public string UserId { get; set; }

        public string UserFullName { get; set; }
    }
}
