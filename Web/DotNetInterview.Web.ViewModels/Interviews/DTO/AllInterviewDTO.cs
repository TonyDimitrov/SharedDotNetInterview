namespace DotNetInterview.Web.ViewModels.Interviews.DTO
{
    using System;

    using DotNetInterview.Web.ViewModels.Enums;

    public class AllInterviewDTO
    {
        public string InterviewId { get; set; }

        public string PositionTitle { get; set; }

        public int Questions { get; set; }

        public DateTime Date { get; set; }

        public PositionSeniorityImgVM PositionSeniority { get; set; }

        public int Likes { get; set; }

        public string CreatorId { get; set; }

        public string CreatorFName { get; set; }

        public string CreatorLName { get; set; }

        public string CreatorAvatar { get; set; }

        public string CreatorUsername { get; set; }
    }
}
