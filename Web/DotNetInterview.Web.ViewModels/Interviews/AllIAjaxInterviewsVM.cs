namespace DotNetInterview.Web.ViewModels.Interviews
{
    using System;

    using DotNetInterview.Web.ViewModels.Common;

    public class AllIAjaxInterviewsVM : PaginationVM
    {
        public AllIAjaxInterviewsVM(int seniority)
        {
            this.Seniority = seniority;
        }

        public int Seniority { get; set; }

        public int? Page { get; set; }

        public int? NationalityId { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }
    }
}
