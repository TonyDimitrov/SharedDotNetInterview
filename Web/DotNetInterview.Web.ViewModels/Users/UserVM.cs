namespace DotNetInterview.Web.ViewModels.Users
{
    using AutoMapper;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Services.Mapping;
    using DotNetInterview.Web.ViewModels.Enums;

    public class UserVM : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string FullName { get; set; }

        public string Email { get; set; }

        public WorkPositionVM Position { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, UserVM>().ForMember(
                m => m.FullName,
                opt => opt.MapFrom(x => x.FirstName + " " + x.LastName));
        }
    }
}
