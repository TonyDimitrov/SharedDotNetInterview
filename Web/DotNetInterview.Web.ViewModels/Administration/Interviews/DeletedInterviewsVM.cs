namespace DotNetInterview.Web.ViewModels.Administration.Interviews
{
    using System.Collections.Generic;

    using DotNetInterview.Web.ViewModels.Common;

    public class DeletedInterviewsVM : PaginationVM
    {
        public IEnumerable<DeletedInterviewVM> DeletedInterviews { get; set; }
    }
}
