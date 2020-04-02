namespace DotNetInterview.Web.ViewModels.Users.DTO
{
    using System;
    using System.Collections.Generic;

    using DotNetInterview.Data.Models;
    using DotNetInterview.Web.ViewModels.Enums;

    public class DetailsUserDTO
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Nationality { get; set; }

        public PersonSeniorityVM Position { get; set; }

        public string Description { get; set; }

        public DateTime MemberSince { get; set; }

        public string Image { get; set; }

        public IEnumerable<DetailsUserInterviewsVM> Interviews { get; set; }
    }
}
