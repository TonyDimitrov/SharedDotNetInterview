﻿namespace DotNetInterview.Web.ViewModels.Users.DTO
{
    using System;

    using DotNetInterview.Web.ViewModels.Enums;
    using Microsoft.AspNetCore.Http;

    public class UpdateUserDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string NationalityId { get; set; }

        public PersonSeniorityVM Position { get; set; }

        public string Description { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateTime? DeletedOn { get; set; }

        public IFormFile Image { get; set; }
    }
}
