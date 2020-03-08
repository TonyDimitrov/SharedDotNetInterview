using DotNetInterview.Web.ViewModels.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetInterview.Web.ViewModels.Users.DTO
{
    public class UpdateUserDTO
    {
        public string LastName { get; set; }

        public string Nationality { get; set; }

        public PositionSeniorityVM Position { get; set; }

        public string Description { get; set; }

        public DateTime? DeletedOn { get; set; }

        public IFormFile Image { get; set; }

    }
}
