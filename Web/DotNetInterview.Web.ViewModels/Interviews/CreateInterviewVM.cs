namespace DotNetInterview.Web.ViewModels.Interviews
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using DotNetInterview.Web.ViewModels.Enums;

    using static DotNetInterview.Web.ViewModels.Constants.DataConstantVM;

    public class CreateInterviewVM
    {
        public CreateInterviewVM()
        {
            this.Questions = new List<CreateInterviewQuestionVM>();
        }

        [Required]
        [Display(Name = "Position seniority")]
        public PositionSeniorityVM Seniority { get; set; }

        [Required]
        [MinLength(PositionTitleMinLength)]
        [MaxLength(PositionTitleMaxLength)]
        [Display(Name = "Position title")]
        public string PositionTitle { get; set; }

        [MinLength(PositionDescriptionMinLength)]
        [MaxLength(PositionDescriptionMaxLength)]
        [Display(Name = "Position description")]
        public string PositionDescription { get; set; }

        [Required]
        [MinLength(LocationTypeMinLength)]
        [MaxLength(LocationTypeMaxLength)]
        [Display(Name = "Location type spesification")]
        public string LocationType { get; set; }

        [MinLength(LocationTypeMinLength)]
        [MaxLength(LocationTypeMaxLength)]
        [Display(Name = "Specify where position is based")]
        public string InterviewLocation { get; set; }

        [Required]
        //[MinLength(LocationTypeMinLength)]
        //[MaxLength(LocationTypeMaxLength)]
        [Display(Name = "Company nationality")]
        public string CompanyNationality { get; set; }

        [Display(Name = "Company size of employees")]
        public EmployeesSizeVM Employees { get; set; }

        public string Tags { get; set; }

        public int Likes { get; set; }

        public List<CreateInterviewQuestionVM> Questions { get; set; }

        public GetCreateInterviewsVM Select { get; set; }
    }
}
