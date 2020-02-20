// ReSharper disable VirtualMemberCallInConstructor
namespace DotNetInterview.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using DotNetInterview.Data.Common.Models;
    using DotNetInterview.Data.Models.Enums;
    using Microsoft.AspNetCore.Identity;

    using static DotNetInterview.Data.Common.Constants.DataConstant;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
            this.Images = new HashSet<Image>();
            this.Interviews = new HashSet<Interview>();
            this.Comments = new HashSet<Comment>();
        }

        [Required]
        [MinLength(NameMinLength)]
        [MaxLength(NameMaxLength)]
        public string FirstName { get; set; }

        [MinLength(NameMinLength)]
        [MaxLength(NameMaxLength)]
        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public Country Country { get; set; }

        public WorkPosition Position { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public virtual ICollection<Image> Images { get; set; }

        public virtual ICollection<Interview> Interviews { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
