namespace DotNetInterview.Web.ViewModels.Administration.Interviews
{
    using System;
    using System.Globalization;

    using AutoMapper;
    using DotNetInterview.Common;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Services.Mapping;
    using DotNetInterview.Web.ViewModels.Enums;

    public class DeletedInterviewVM : IMapFrom<Interview>, IHaveCustomMappings
    {
        public string InterviewId { get; set; }

        public string PositionTitle { get; set; }

        public int Questions { get; set; }

        public PositionSeniorityVM Seniority { get; set; }

        public string CreatedOn { get; set; }

        public string DeletedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Interview, DeletedInterviewVM>()
                .ForMember(
            i => i.InterviewId,
            opt => opt.MapFrom(x => x.Id))
                .ForMember(
            i => i.CreatedOn,
            opt => opt.MapFrom(x => x.CreatedOn.ToString(GlobalConstants.FormatDate, CultureInfo.InvariantCulture)))
                .ForMember(
            i => i.DeletedOn,
            opt => opt.MapFrom(x => x.DeletedOn != null ? x.DeletedOn.Value.ToString(GlobalConstants.FormatDate, CultureInfo.InvariantCulture) : null))
                .ForMember(
            i => i.PositionTitle,
            opt => opt.MapFrom(x => x.PositionTitle != null && x.PositionTitle.Length <= 50 ? x.PositionTitle : x.PositionTitle.Substring(0, 47) + "..."))
                    .ForMember(
            i => i.Seniority,
            opt => opt.MapFrom(x => Enum.Parse<PositionSeniorityVM>(x.Seniority.ToString())))
                    .ForMember(
            i => i.Questions,
            opt => opt.MapFrom(x => x.Questions.Count));
        }
    }
}
