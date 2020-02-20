﻿namespace DotNetInterview.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using DotNetInterview.Data.Common.Models;
    using DotNetInterview.Data.Models.Enums;

    using static DotNetInterview.Data.Common.Constants.DataConstant;

    public class Interview : BaseDeletableModel<string>
    {

        public Interview()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Questions = new HashSet<Question>();
            this.Comments = new HashSet<Comment>();
        }

        [Required]
        [MinLength(PositionTitleMinLength)]
        [MaxLength(PositionTitleMaxLength)]
        public string PositionTitle { get; set; }

        [MinLength(PositionDescriptionMinLength)]
        [MaxLength(PositionDescriptionMaxLength)]
        public string PositionDescription { get; set; }

        public LocationType LocationType { get; set; }

        [MinLength(LocationlMinLength)]
        [MaxLength(LocationlMaxLength)]
        public string HeldOnInterviewLocation { get; set; }

        public EmployeesSize Employees { get; set; }

        public DateTime HeldOnDate { get; set; }

        [MaxLength(TagsMaxLength)]
        public string Tags { get; set; }

        public int Likes { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public virtual ICollection<Question> Questions { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
