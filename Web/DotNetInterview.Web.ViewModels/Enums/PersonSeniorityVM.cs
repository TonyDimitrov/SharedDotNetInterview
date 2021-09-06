namespace DotNetInterview.Web.ViewModels.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum PersonSeniorityVM
    {
        [Display(Name = "All")]
        All = 0,
        [Display(Name = "Junior developer")]
        JuniorDeveloper = 1,
        [Display(Name = "Regular developer")]
        RegularDeveloper = 2,
        [Display(Name = "Senior developer")]
        SeniorDeveloper = 3,
        [Display(Name = "Lead developer")]
        LeadDeveloper = 4,
        [Display(Name = "Technical architect")]
        TechnicalArchitect = 5,
        [Display(Name = "Other")]
        Other = 99,
    }
}
