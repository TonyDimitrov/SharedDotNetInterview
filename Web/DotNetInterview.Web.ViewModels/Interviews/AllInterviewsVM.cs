namespace DotNetInterview.Web.ViewModels.Interviews
{
    using System;
    using System.Collections.Generic;

    using DotNetInterview.Web.ViewModels.Common;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class AllInterviewsVM : PaginationVM
    {
        public AllInterviewsVM(int seniority)
        {
            this.Seniority = seniority;
        }

        public int Seniority { get; set; }

        public int? NationalityId { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public IEnumerable<SelectListItem> Nationalities { get; set; }

        public IEnumerable<InterviewVM> Interviews { get; set; }
    }
}
