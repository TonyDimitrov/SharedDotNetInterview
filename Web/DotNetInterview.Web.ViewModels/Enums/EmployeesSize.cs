namespace DotNetInterview.Web.ViewModels.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum EmployeesSize
    {
        [Display(Name = "More than 1000")]
        MoreThan1000 = 4,
        [Display(Name = "None")]
        None = 0,
        [Display(Name = "Les than 10")]
        LesThan10 = 1,
        [Display(Name = "Between 10 and 100")]
        Between10And100 = 2,
        [Display(Name = "Between 100 and 1000")]
        Between100And1000 = 3,
    }
}
