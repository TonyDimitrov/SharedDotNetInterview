using DotNetInterview.Web.ViewModels.Enums;
using System.Collections.Generic;

namespace DotNetInterview.Web.ViewModels.Interviews
{
   public class DetailsInterviewVM
    {
        public string UserId { get; set; }

        public string InterviewId { get; set; }

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

        public IEnumerable<AllInterviewCommentsVM> QnsComments { get; set; }
    }
}
