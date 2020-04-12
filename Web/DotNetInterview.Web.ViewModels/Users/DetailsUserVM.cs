namespace DotNetInterview.Web.ViewModels.Users
{
    using System;
    using System.Collections.Generic;

    using DotNetInterview.Web.ViewModels.Enums;

    public class DetailsUserVM
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string DateOfBirth { get; set; }

        public string Nationality { get; set; }

        public string Position { get; set; }

        public string Description { get; set; }

        public string MemberSince { get; set; }

        public virtual string Image { get; set; }

        public string ShowEdit { get; set; }

        public string ShowDelete { get; set; }

        public virtual IEnumerable<DetailsUserInterviewsVM> Interviews { get; set; }
    }
}
