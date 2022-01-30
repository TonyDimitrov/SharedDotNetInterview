namespace DotNetInterview.Web.ViewModels.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum CurrencyTypeVM
    {
        [Display(Name = "EUR")]
        EUR = 1,
        [Display(Name = "USD")]
        USD = 2,
    }
}
