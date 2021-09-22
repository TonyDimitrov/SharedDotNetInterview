namespace DotNetInterview.Web.ViewModels.Interviews.DTO
{
    using System;

    public class AllAjaxInterviewDTO
    {
        public int Seniority { get; set; }

        public int? Page { get; set; }

        public int? NationalityId { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }
    }
}
