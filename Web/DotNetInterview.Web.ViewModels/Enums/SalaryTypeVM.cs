namespace DotNetInterview.Web.ViewModels.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum SalaryTypeVM
    {
        [Display(Name = "Per month")]
        Month = 1,
        [Display(Name = "Per year")]
        Year = 2,
    }
}
