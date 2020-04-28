namespace DotNetInterview.Web.ViewModels.Interviews.DTO
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using DotNetInterview.Web.ViewModels.Enums;
    using Microsoft.AspNetCore.Mvc.Rendering;

    using static DotNetInterview.Web.ViewModels.Constants.DataConstantVM;

    public class EditInterviewDTO
    {
        public EditInterviewDTO()
        {
            this.Questions = new List<EditInterviewQuestionsDTO>();
        }

        [Required]
        public string InterviewId { get; set; }

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
        [Display(Name = "Location type spesification")]
        public LocationTypeVM LocationType { get; set; }

        public string InOfficeChecked { get; set; }

        public string RemoteChecked { get; set; }

        public string ShowLocation { get; set; }

        [MinLength(LocationTypeMinLength, ErrorMessage = "Position location should be minimum 2 characters!")]
        [MaxLength(LocationTypeMaxLength, ErrorMessage = "Position location should be maximum 2 characters!")]
        [Display(Name = "Specify where position is based")]
        public string BasedPositionLocation { get; set; }

        [Display(Name = "Company nationality")]
        public string CompanyNationality { get; set; }

        public IEnumerable<SelectListItem> CompanyListNationalities { get; set; }

        public List<EditInterviewQuestionsDTO> Questions { get; set; }

        public PositionSeniorityVM SenioritiesCollection { get; set; }

        [Display(Name = "Total employees")]
        public EmployeesSizeVM Employees { get; set; }
    }
}
