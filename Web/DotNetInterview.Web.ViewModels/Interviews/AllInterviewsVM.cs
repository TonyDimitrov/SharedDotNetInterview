namespace DotNetInterview.Web.ViewModels.Interviews
{
    using System.Collections.Generic;

    public class AllInterviewsVM
    {
        public int Seniority { get; set; }

        public int PaginationLength { get; set; } = 3;

        public int StartrIndex { get; set; }

        public int CurrentSet { get; set; }

        public string PrevtDisable { get; set; }

        public string NextDisable { get; set; }

        public IEnumerable<InterviewVM> Interviews { get; set; }
    }
}
