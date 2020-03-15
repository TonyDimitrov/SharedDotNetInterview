namespace DotNetInterview.Web.ViewModels.Interviews
{
    using System.Collections.Generic;

    using DotNetInterview.Web.ViewModels.Comments;

    public class DetailsInterviewVM
    {
        public DetailsInterviewVM()
        {
            this.InterviewQns = new List<AllInterviewQuestionsVM>();
            this.InterviewComments = new List<AllCommentsVM>();
        }

        public string UserId { get; set; }

        public string InterviewId { get; set; }

        public string Seniority { get; set; }

        public string PositionTitle { get; set; }

        public string PositionDescription { get; set; }

        public string LocationType { get; set; }

        public string InterviewLocation { get; set; }

        public string CompanyNationality { get; set; }

        public string CompanySize { get; set; }

        public string CreatedOn { get; set; }

        public string ModifiedOn { get; set; }

        public int Likes { get; set; }

        public IEnumerable<AllInterviewQuestionsVM> InterviewQns { get; set; }

        public IEnumerable<AllCommentsVM> InterviewComments { get; set; }
    }
}
