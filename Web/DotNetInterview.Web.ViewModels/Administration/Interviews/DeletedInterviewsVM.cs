namespace DotNetInterview.Web.ViewModels.Administration.Interviews
{
    using System.Collections.Generic;

    public class DeletedInterviewsVM
    {
        public IEnumerable<DeletedInterviewVM> DeletedInterviews { get; set; }
    }
}
