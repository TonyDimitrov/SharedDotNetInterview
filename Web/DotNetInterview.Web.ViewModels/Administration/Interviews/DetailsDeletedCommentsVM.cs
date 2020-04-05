namespace DotNetInterview.Web.ViewModels.Administration.Interviews
{
    using System.Globalization;

    using AutoMapper;
    using DotNetInterview.Common;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Services.Mapping;

    public class DetailsDeletedCommentsVM : IMapFrom<Comment>, IHaveCustomMappings
    {
        public string HideDelete => "hidden";

        public string HideAdd => "hidden";

        public string ParentId { get; set; }

        public string Content { get; set; }

        public string CreatedOn { get; set; }

        public string DeletedOn { get; set; }

        public string UserId { get; set; }

        public string UserFullName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Comment, DetailsDeletedCommentsVM>()
                .ForMember(c => c.ParentId, opt => opt.MapFrom(c => c.Id))
                .ForMember(c => c.CreatedOn, opt => opt.MapFrom(c => c.CreatedOn.ToString(GlobalConstants.FormatDate, CultureInfo.InvariantCulture)))
                .ForMember(c => c.DeletedOn, opt => opt.MapFrom(c => c.DeletedOn != null ? c.DeletedOn.Value.ToString(GlobalConstants.FormatDate, CultureInfo.InvariantCulture) : null))
                .ForMember(c => c.UserId, opt => opt.MapFrom(c => c.UserId))
                .ForMember(c => c.UserFullName, opt => opt.MapFrom(c =>
                string.IsNullOrWhiteSpace(c.User.LastName)
                    ? c.User.FirstName.Length <= 20
                        ? c.User.FirstName
                        : c.User.FirstName.Substring(0, 17) + "..."
                     : (c.User.FirstName + " " + c.User.LastName.Substring(0, 1).ToUpper() + ".").Length <= 20
                         ? c.User.FirstName + " " + c.User.LastName.Substring(0, 1).ToUpper() + "."
                         : (c.User.FirstName + " " + c.User.LastName.Substring(0, 1).ToUpper()).Substring(0, 17) + "..."));
        }
    }
}
