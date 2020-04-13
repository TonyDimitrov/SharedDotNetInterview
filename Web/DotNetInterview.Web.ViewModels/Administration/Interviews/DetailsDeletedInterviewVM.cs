namespace DotNetInterview.Web.ViewModels.Administration.Interviews
{
    using System.Collections.Generic;
    using System.Globalization;

    using AutoMapper;
    using DotNetInterview.Common;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Data.Models.Enums;
    using DotNetInterview.Services.Mapping;

    public class DetailsDeletedInterviewVM : IMapFrom<Interview>, IHaveCustomMappings
    {
        public DetailsDeletedInterviewVM()
        {
            this.Questions = new List<DetailsDeletedInterviewQuestionsVM>();
            this.Comments = new List<DetailsDeletedCommentsVM>();
        }

        public string UserId { get; set; }

        public string UserFullName { get; set; }

        public string InterviewId { get; set; }

        public string Seniority { get; set; }

        public string PositionTitle { get; set; }

        public string PositionDescription { get; set; }

        public string LocationType { get; set; }

        public string ShowLocation { get; set; }

        public string BasedPositionLocation { get; set; }

        public string CompanyNationality { get; set; }

        public string CompanySize { get; set; }

        public string CreatedOn { get; set; }

        public string DeletedOn { get; set; }

        public int Likes { get; set; }

        public string HideAddCommentForm => "hidden";

        public string CanEdit => "hidden";

        public string CanDelete => "hidden";

        public string CanHardDelete => "hidden";

        public string Deleted => "Deleted";

        public IEnumerable<DetailsDeletedInterviewQuestionsVM> Questions { get; set; }

        public IEnumerable<DetailsDeletedCommentsVM> Comments { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Comment, DetailsDeletedCommentsVM>()
                .ForMember(c => c.ParentId, opt => opt.MapFrom(c => c.Id))
                .ForMember(c => c.CreatedOn, opt => opt.MapFrom(c => c.CreatedOn.ToLocalTime().ToString(GlobalConstants.FormatDate, CultureInfo.InvariantCulture)))
                .ForMember(c => c.DeletedOn, opt => opt.MapFrom(c => c.DeletedOn != null ? c.DeletedOn.Value.ToLocalTime().ToString(GlobalConstants.FormatDate, CultureInfo.InvariantCulture) : null))
                .ForMember(c => c.UserId, opt => opt.MapFrom(c => c.UserId))
                .ForMember(c => c.UserFullName, opt => opt.MapFrom(c =>
                string.IsNullOrWhiteSpace(c.User.LastName)
                    ? c.User.FirstName.Length <= 20
                        ? c.User.FirstName
                        : c.User.FirstName.Substring(0, 17) + "..."
                     : (c.User.FirstName + " " + c.User.LastName.Substring(0, 1).ToUpper() + ".").Length <= 20
                         ? c.User.FirstName + " " + c.User.LastName.Substring(0, 1).ToUpper() + "."
                         : (c.User.FirstName + " " + c.User.LastName.Substring(0, 1).ToUpper()).Substring(0, 17) + "..."));

            configuration.CreateMap<Question, DetailsDeletedInterviewQuestionsVM>()
                .ForMember(q => q.Answer, opt => opt.MapFrom(q => q.GivenAnswer))
                .ForMember(q => q.HideAnswer, opt => opt.MapFrom(q => q.GivenAnswer != null ? string.Empty : "hidden"))
                .ForMember(q => q.Ranked, opt => opt.MapFrom(q => q.RankType.ToString()))
                .ForMember(q => q.HideRanked, opt => opt.MapFrom(q => q.RankType != QuestionRankType.None ? string.Empty : "hidden"))
                .ForMember(q => q.CreatedOn, opt => opt.MapFrom(q => q.CreatedOn.ToLocalTime()
                .ToString(GlobalConstants.FormatDate, CultureInfo.InvariantCulture)))
                .ForMember(q => q.DeletedOn, opt => opt.MapFrom(q => q.DeletedOn != null
                ? q.DeletedOn.Value.ToLocalTime().ToString(GlobalConstants.FormatDate, CultureInfo.InvariantCulture) : null))
                .ForMember(q => q.File, opt => opt.MapFrom(q => q.UrlTask))
                .ForMember(q => q.HideFile, opt => opt.MapFrom(q => !string.IsNullOrEmpty(q.UrlTask) ? string.Empty : "hidden"))
                .ForMember(q => q.Comments, opt => opt.MapFrom(q => q.Comments));

            configuration.CreateMap<Interview, DetailsDeletedInterviewVM>()
                .ForMember(i => i.InterviewId, opt => opt.MapFrom(i => i.Id))
                .ForMember(i => i.UserFullName, opt => opt.MapFrom(i =>
                string.IsNullOrWhiteSpace(i.User.LastName)
                    ? i.User.FirstName.Length <= 20
                        ? i.User.FirstName
                        : i.User.FirstName.Substring(0, 17) + "..."
                     : (i.User.FirstName + " " + i.User.LastName.Substring(0, 1).ToUpper() + ".").Length <= 20
                         ? i.User.FirstName + " " + i.User.LastName.Substring(0, 1).ToUpper() + "."
                         : (i.User.FirstName + " " + i.User.LastName.Substring(0, 1).ToUpper()).Substring(0, 17) + "..."))
                .ForMember(i => i.Seniority, opt => opt.MapFrom(i => i.Seniority.ToString()))
                .ForMember(i => i.PositionDescription, opt => opt.MapFrom(i => i.PositionDescription != null ? i.PositionDescription : GlobalConstants.NoDescription))
                .ForMember(i => i.LocationType, opt => opt.MapFrom(i => i.LocationType.ToString()))
                .ForMember(i => i.ShowLocation, opt => opt.MapFrom(i =>
                i.LocationType == Data.Models.Enums.LocationType.InOffice
                ? string.Empty : "hidden"))
                .ForMember(i => i.BasedPositionLocation, opt => opt.MapFrom(i => i.BasedPositionLocation))
                .ForMember(i => i.CompanySize, opt => opt.MapFrom(i => i.Employees.ToString()))
                .ForMember(i => i.CreatedOn, opt => opt.MapFrom(i => i.CreatedOn.ToLocalTime().ToString(GlobalConstants.FormatDate, CultureInfo.InvariantCulture)))
                .ForMember(i => i.DeletedOn, opt => opt.MapFrom(i => i.DeletedOn != null ? i.DeletedOn.Value.ToLocalTime().ToString(GlobalConstants.FormatDate, CultureInfo.InvariantCulture) : null))
                .ForMember(i => i.Likes, opt => opt.MapFrom(i => i.Likes.Count))
                .ForMember(i => i.Comments, opt => opt.MapFrom(i => i.Comments))
                .ForMember(i => i.Questions, opt => opt.MapFrom(i => i.Questions));
        }
    }
}
