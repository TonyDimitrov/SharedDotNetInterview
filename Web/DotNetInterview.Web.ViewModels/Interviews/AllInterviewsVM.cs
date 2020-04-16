namespace DotNetInterview.Web.ViewModels.Interviews
{
    using System.Collections.Generic;

    using DotNetInterview.Web.ViewModels.Common;

    public class AllInterviewsVM : PaginationVM
    {
        public AllInterviewsVM(int seniority)
        {
            this.Seniority = seniority;
        }

        public int Seniority { get; set; }

        public IEnumerable<InterviewVM> Interviews { get; set; }
    }
}
