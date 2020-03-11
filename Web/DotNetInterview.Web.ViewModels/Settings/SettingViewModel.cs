namespace DotNetInterview.Web.ViewModels.Settings
{
    using System.Collections;
    using System.Collections.Generic;

    using AutoMapper;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Services.Mapping;

    public class SettingViewModel : IMapFrom<Setting>, IHaveCustomMappings
    {
        public SettingViewModel()
        {
            this.UserEmails = new List<string>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string NameAndValue { get; set; }

        public IList<string> UserEmails { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Setting, SettingViewModel>().ForMember(
                m => m.NameAndValue,
                opt => opt.MapFrom(x => x.Name + " = " + x.Value));
        }
    }
}
