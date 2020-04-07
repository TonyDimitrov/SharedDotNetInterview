namespace DotNetInterview.Web.ViewModels.Administration.Users
{
    using System.Globalization;

    using AutoMapper;
    using DotNetInterview.Common;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Services.Mapping;

    public class DetailsDeletedUser : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string DateOfBirth { get; set; }

        public string Nationality { get; set; }

        public string Position { get; set; }

        public string Description { get; set; }

        public string MemberSince { get; set; }

        public string DeletedOn { get; set; }

        public string Image { get; set; }

        public string Status { get; set; }

        public int Shares { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, DetailsDeletedUser>()
                .ForMember(u => u.FullName, opt => opt.MapFrom(u => u.FirstName + " " + u.LastName))
                .ForMember(u => u.DateOfBirth, opt => opt.MapFrom(u => u.DateOfBirth != null
                ? u.DateOfBirth.Value.ToLocalTime()
                .ToString(GlobalConstants.FormatDate, CultureInfo.InvariantCulture) : GlobalConstants.NoInformation))
                .ForMember(u => u.Description, opt => opt.MapFrom(u => u.Description == null ? GlobalConstants.NoDescription : u.Description))
                .ForMember(u => u.MemberSince, opt => opt.MapFrom(u => u.CreatedOn.ToLocalTime()
                .ToString(GlobalConstants.FormatDate, CultureInfo.InvariantCulture)))
                .ForMember(u => u.DeletedOn, opt => opt.MapFrom(u => u.DeletedOn != null
                ? u.DeletedOn.Value.ToLocalTime().ToString(GlobalConstants.FormatDate, CultureInfo.InvariantCulture) : null))
                .ForMember(u => u.Status, opt => opt.MapFrom(u => u.IsDeleted ? GlobalConstants.DeletedStatus : GlobalConstants.ActiveStatus ))
                .ForMember(u => u.Shares, opt => opt.MapFrom(u => u.Interviews.Count));
        }
    }
}
