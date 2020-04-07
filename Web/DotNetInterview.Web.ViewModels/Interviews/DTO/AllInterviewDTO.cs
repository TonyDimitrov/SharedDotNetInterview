using DotNetInterview.Web.ViewModels.Enums;

namespace DotNetInterview.Web.ViewModels.Interviews.DTO
{
    public class AllInterviewDTO
    {
        public string InterviewId { get; set; }

        public string PositionTitle { get; set; }

        public int Questions { get; set; }

        public string Date { get; set; }

        public string PositionSeniority { get; set; }

        public int Likes { get; set; }

        public string CreatorId { get; set; }

        public string CreatorFName { get; set; }

        public string CreatorLName { get; set; }

        public string CreatorAvatar { get; set; }

        public string CreatorUsername { get; set; }
    }
}
