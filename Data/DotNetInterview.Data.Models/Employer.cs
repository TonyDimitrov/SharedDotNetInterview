namespace DotNetInterview.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using DotNetInterview.Data.Common.Models;

    using static DotNetInterview.Data.Common.Constants.DataConstant;

    public class Employer : BaseDeletableModel<string>
    {
        public Employer()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [MinLength(NameEmpMinLength)]
        [MaxLength(NameEmpMaxLength)]
        [Required]
        public string Name { get; set; }

        public ICollection<Interview> Interviews { get; set; } = new List<Interview>();
    }
}
