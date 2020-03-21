namespace DotNetInterview.Web.ViewModels.Interviews
{
    using System.Collections.Generic;

    using DotNetInterview.Web.ViewModels.Enums;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class GetCreateInterviewsVM
    {
        public IEnumerable<SelectListItem> Nationality { get; set; }

        public PositionSeniorityVM SenioritiesCollection { get; set; }
    }
}
