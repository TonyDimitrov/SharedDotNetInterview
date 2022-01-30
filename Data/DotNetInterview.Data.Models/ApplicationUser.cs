// ReSharper disable VirtualMemberCallInConstructor
namespace DotNetInterview.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

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
            this.Interviews = new HashSet<Interview>();
            this.Comments = new HashSet<Comment>();
            this.Answers = new HashSet<Answer>();
        }

        [Required]
        [MinLength(NameMinLength)]
        [MaxLength(NameMaxLength)]
        public string FirstName { get; set; }

        [MinLength(NameMinLength)]
        [MaxLength(NameMaxLength)]
        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [MinLength(NationalityMinLength)]
        [MaxLength(NationalityMaxLength)]
        [Column("UserNationality")]
        public string UserNationality { get; set; }

        public WorkPosition Position { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public int? NationalityId { get; set; }

        public Nationality Nationality { get; set; }

        public virtual string Image { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public virtual ICollection<Interview> Interviews { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
    }
}
