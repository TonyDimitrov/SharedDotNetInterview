namespace DotNetInterview.Web.ViewModels.Users.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using DotNetInterview.Web.ViewModels.Enums;
    using Microsoft.AspNetCore.Http;

    public class UpdateUserDTO
    {
        public string LastName { get; set; }

        public string Nationality { get; set; }

        public PersonSeniorityVM Position { get; set; }

        public string Description { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateTime? DeletedOn { get; set; }

        public IFormFile Image { get; set; }

    }
}
