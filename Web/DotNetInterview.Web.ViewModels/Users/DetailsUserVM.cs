namespace DotNetInterview.Web.ViewModels.Users
{
    using System;
    using System.Collections.Generic;

    using DotNetInterview.Web.ViewModels.Enums;

    public class DetailsUserVM
    {
        public string FullName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Nationality { get; set; }

        public PositionSeniorityVM Position { get; set; }

        public string Description { get; set; }

        public DateTime MemberSince { get; set; }

        public virtual string Image { get; set; }

        public virtual IEnumerable<DetailsUserInterviewsVM> Interviews { get; set; }
    }
}
