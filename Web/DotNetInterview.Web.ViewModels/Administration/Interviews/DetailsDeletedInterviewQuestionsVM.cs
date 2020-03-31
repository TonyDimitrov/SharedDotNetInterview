namespace DotNetInterview.Web.ViewModels.Administration.Interviews
{
    using System.Collections.Generic;
    using System.Globalization;

    using AutoMapper;
    using DotNetInterview.Common;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Data.Models.Enums;
    using DotNetInterview.Services.Mapping;

    public class DetailsDeletedInterviewQuestionsVM : IMapFrom<Question>, IHaveCustomMappings
    {
        public DetailsDeletedInterviewQuestionsVM() => this.Comments = new List<DetailsDeletedCommentsVM>();

        public string Content { get; set; }

        public string HideAnswer { get; set; }

        public string Answer { get; set; }

        public string HideRanked { get; set; }

        public string Ranked { get; set; }

        public string CreatedOn { get; set; }

        public string DeletedOn { get; set; }

        public bool HideFile { get; set; }

        public string File { get; set; }

        public IEnumerable<DetailsDeletedCommentsVM> Comments { get; set; }

        public QuestionRankType RankType { get; private set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Question, DetailsDeletedInterviewQuestionsVM>()
                .ForMember(q => q.Answer, opt => opt.MapFrom(q => q.GivenAnswer))
                .ForMember(q => q.HideAnswer, opt => opt.MapFrom(q => q.GivenAnswer != null ? string.Empty : "hidden"))
                .ForMember(q => q.Ranked, opt => opt.MapFrom(q => q.RankType.ToString()))
                .ForMember(q => q.HideRanked, opt => opt.MapFrom(q => q.RankType != QuestionRankType.None ? string.Empty : "hidden"))
                .ForMember(q => q.CreatedOn, opt => opt.MapFrom(q => q.CreatedOn.ToString(GlobalConstants.FormatDate, CultureInfo.InvariantCulture)))
                .ForMember(q => q.DeletedOn, opt => opt.MapFrom(q => q.DeletedOn != null
                ? q.DeletedOn.Value.ToString(GlobalConstants.FormatDate, CultureInfo.InvariantCulture) : null))
                .ForMember(q => q.File, opt => opt.MapFrom(q => q.UrlTask))
                .ForMember(q => q.HideFile, opt => opt.MapFrom(q => string.IsNullOrEmpty(q.UrlTask) ? string.Empty : "hidden"));
        }
    }
}
