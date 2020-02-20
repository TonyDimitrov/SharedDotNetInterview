namespace DotNetInterview.Data.Models
{
    using System;

    using DotNetInterview.Data.Common.Models;

    public class Image : BaseDeletableModel<string>
    {
        public Image()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string ImageUrl { get; set; }

        public bool Private { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
