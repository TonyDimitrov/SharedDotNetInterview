namespace DotNetInterview.Web.ViewModels.Interviews.DTO
{
    using System;
    using System.Collections.Generic;

    using DotNetInterview.Web.ViewModels.Enums;

    public class DetailsInterviewDTO
    {
        public string UserId { get; set; }

        public string InterviewId { get; set; }

        public string PositionTitle { get; set; }

        public string PositionDescription { get; set; }

        public LocationTypeVM LocationType { get; set; }

        public string InterviewLocation { get; set; }

        public string CompanyNationality { get; set; }

        public EmployeesSizeVM CompanySize { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int Likes { get; set; }

        public IEnumerable<AllInterviewQuestionsDTO> InterviewQns { get; set; }

        public IEnumerable<AllInterviewCommentsDTO> QnsComments { get; set; }
    }
}
