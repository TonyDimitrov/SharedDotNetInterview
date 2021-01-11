namespace DotNetInterview.Web.ViewModels.Interviews.DTO
{
    using System;
    using System.Collections.Generic;

    using DotNetInterview.Web.ViewModels.Comments.DTO;
    using DotNetInterview.Web.ViewModels.Enums;

    public class DetailsInterviewDTO
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string UserFName { get; set; }

        public string UserLName { get; set; }

        public string InterviewId { get; set; }

        public string Seniority { get; set; }

        public PositionSeniorityImgVM SeniorityImg { get; set; }

        public string PositionTitle { get; set; }

        public string PositionDescription { get; set; }

        public string LocationType { get; set; }

        public string InterviewLocation { get; set; }

        public string CompanyNationality { get; set; }

        public string CompanySize { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int Likes { get; set; }

        public bool IsLiked { get; set; }

        public IEnumerable<AllInterviewQuestionsDTO> InterviewQns { get; set; }

        public IEnumerable<AllCommentsDTO> InterviewComments { get; set; }
    }
}
