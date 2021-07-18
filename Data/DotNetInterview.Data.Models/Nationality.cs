namespace DotNetInterview.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static DotNetInterview.Data.Common.Constants.DataConstant;

    public class Nationality
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(NationalityMinLength)]
        [MaxLength(NationalityMaxLength)]
        public string CompanyNationality { get; set; }
    }
}
