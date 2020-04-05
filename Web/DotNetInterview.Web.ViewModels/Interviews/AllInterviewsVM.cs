namespace DotNetInterview.Web.ViewModels.Interviews
{
    using System.Collections.Generic;

    using DotNetInterview.Web.ViewModels.Common;

    public class AllInterviewsVM : PaginationVM
    {
        public int Seniority { get; set; }

        public IEnumerable<InterviewVM> Interviews { get; set; }
    }
}
