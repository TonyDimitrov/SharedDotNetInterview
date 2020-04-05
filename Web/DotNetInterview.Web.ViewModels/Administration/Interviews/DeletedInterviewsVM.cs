namespace DotNetInterview.Web.ViewModels.Administration.Interviews
{
    using System.Collections.Generic;

    public class DeletedInterviewsVM
    {
        public int Seniority { get; set; }

        public int PaginationLength { get; set; } = 3;

        public int StartrIndex { get; set; }

        public int CurrentSet { get; set; }

        public string PrevtDisable { get; set; }

        public string NextDisable { get; set; }

        public IEnumerable<DeletedInterviewVM> DeletedInterviews { get; set; }
    }
}
