namespace DotNetInterview.Web.ViewModels.Interviews
{
    using System.Collections.Generic;

    public class AllInterviewsVM
    {
        public IEnumerable<InterviewVM> Interviews { get; set; }
    }
}
