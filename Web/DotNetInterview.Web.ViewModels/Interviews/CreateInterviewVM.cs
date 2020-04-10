namespace DotNetInterview.Web.ViewModels.Interviews
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using DotNetInterview.Web.Infrastructure.CustomValidationAttributes;
    using DotNetInterview.Web.ViewModels.Enums;
    using Microsoft.AspNetCore.Mvc.Rendering;

    using static DotNetInterview.Web.ViewModels.Constants.DataConstantVM;

    public class CreateInterviewVM
        {
        public CreateInterviewVM()
        {
            this.Questions = new List<CreateInterviewQuestionVM>();
        }

        [Required(ErrorMessage = "Position seniority required!")]
        [Display(Name = "Position seniority")]
        public PositionSeniorityVM Seniority { get; set; }

        [Required(ErrorMessage = "Position title required!")]
        [MinLength(PositionTitleMinLength, ErrorMessage = "Position title should be minimum 2 characters!")]
        [MaxLength(PositionTitleMaxLength, ErrorMessage = "Position title should be maximum 200 characters!")]
        [Display(Name = "Applying position title")]
        public string PositionTitle { get; set; }

        [MinLength(PositionDescriptionMinLength, ErrorMessage = "Position description should be minimum 2 characters!")]
        [MaxLength(PositionDescriptionMaxLength, ErrorMessage = "Position description should be maximum 5000 characters!")]
        [Display(Name = "Applying position description")]
        public string PositionDescription { get; set; }

        [Required]
        [MinLength(LocationTypeMinLength)]
        [MaxLength(LocationTypeMaxLength)]
        [Display(Name = "Location type spesification")]
        public string LocationType { get; set; }

        [MinLength(LocationTypeMinLength, ErrorMessage = "Position location should be minimum 2 characters!")]
        [MaxLength(LocationTypeMaxLength, ErrorMessage = "Position location should be maximum 2 characters!")]
        [Display(Name = "Specify where position is based")]
        public string BasedPositionLocation { get; set; }

        //[Required]
        //[MinLength(LocationTypeMinLength, ErrorMessage = "Company nationality should be minimum 2 characters!")]
        //[MaxLength(LocationTypeMaxLength, ErrorMessage = "Company nationality should be maximum 2 characters!")]
        [Display(Name = "Company nationality")]
        public string CompanyNationality { get; set; }

        [Display(Name = "Company size of employees")]
        public EmployeesSizeVM Employees { get; set; }

        [CollectionMinLengthAttribute(1)]
        public List<CreateInterviewQuestionVM> Questions { get; set; }

        public IEnumerable<SelectListItem> CompanyListNationalities { get; set; }
    }
}
