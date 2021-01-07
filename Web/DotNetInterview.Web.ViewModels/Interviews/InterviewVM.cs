namespace DotNetInterview.Web.ViewModels.Interviews
{
    using DotNetInterview.Web.ViewModels.Enums;

    public class InterviewVM
    {
        public string InterviewId { get; set; }

        public string PositionTitle { get; set; }

        public int Questions { get; set; }

        public string Date { get; set; }

        public string Seniority { get; set; }

        public int Likes { get; set; }

        public string CreatorId { get; set; }

        public string DisableUserLink { get; set; }

        public string CreatorName { get; set; }

        public string CreatorAvatar { get; set; }
    }
}
