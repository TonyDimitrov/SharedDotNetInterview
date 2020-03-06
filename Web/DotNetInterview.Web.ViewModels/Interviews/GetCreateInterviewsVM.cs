namespace DotNetInterview.Web.ViewModels.Interviews
{
    using System.Collections.Generic;

    using DotNetInterview.Web.ViewModels.Enums;

    public class GetCreateInterviewsVM
    {
        public IEnumerable<string> Nationality { get; set; }

        public PositionSeniorityVM SenioritiesCollection { get; set; }
    }
}
