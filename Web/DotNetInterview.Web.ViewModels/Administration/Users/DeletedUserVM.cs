namespace DotNetInterview.Web.ViewModels.Administration.Users
{
    using System;
    using System.Globalization;

    using AutoMapper;
    using DotNetInterview.Common;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Services.Mapping;

    public class DeletedUserVM : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string Senioriry { get; set; }

        public int Shared { get; set; }

        public string MemberSince { get; set; }

        public string DeletedOn { get; set; }

        public string Image { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, DeletedUserVM>()
             .ForMember(u => u.FullName, opt => opt.MapFrom(u => u.FirstName + " " + u.LastName))
             .ForMember(u => u.Senioriry, opt => opt.MapFrom(u => u.Position))
             .ForMember(u => u.Shared, opt => opt.MapFrom(u => u.Interviews.Count))
             .ForMember(u => u.MemberSince, opt => opt.MapFrom(u => u.CreatedOn.ToLocalTime().ToString(GlobalConstants.FormatDate, CultureInfo.InvariantCulture)))
             .ForMember(u => u.DeletedOn, opt => opt.MapFrom(u => u.DeletedOn != null
             ? u.DeletedOn.Value.ToLocalTime().ToString(GlobalConstants.FormatDate, CultureInfo.InvariantCulture) : null));
        }
    }
}
