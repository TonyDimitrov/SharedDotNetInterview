namespace DotNetInterview.Web.ViewModels.Administration.Nationalities
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public class ManageNationalitiesVM
    {
        public string Delete { get; set; }

        public string Add { get; set; }

        public IEnumerable<SelectListItem> Nationalities { get; set; }
    }
}
