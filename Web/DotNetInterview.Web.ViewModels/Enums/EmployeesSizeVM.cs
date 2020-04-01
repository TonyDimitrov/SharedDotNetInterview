namespace DotNetInterview.Web.ViewModels.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum EmployeesSizeVM
    {
        [Display(Name = "More than 1000")]
        MoreThan1000 = 4,
        [Display(Name = "Less than 10")]
        LessThan10 = 1,
        [Display(Name = "Between 10 and 100")]
        Between10And100 = 2,
        [Display(Name = "Between 100 and 1000")]
        Between100And1000 = 3,
    }
}
