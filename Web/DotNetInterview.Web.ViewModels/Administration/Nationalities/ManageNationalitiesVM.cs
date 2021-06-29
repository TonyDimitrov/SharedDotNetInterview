namespace DotNetInterview.Web.ViewModels.Administration.Nationalities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using DotNetInterview.Web.ViewModels.Constants;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class ManageNationalitiesVM
    {
        public int Id { get; set; }

        public int NationalityId { get; set; }

        [Display(Name = "Delete company nationality")]
        public string Delete { get; set; }

        [Display(Name = "Add company nationality")]
        public string Add { get; set; }

        public string StatusMessage { get; set; }

        public IEnumerable<SelectListItem> Nationalities { get; set; }
    }
}
