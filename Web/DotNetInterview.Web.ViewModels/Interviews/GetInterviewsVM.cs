using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetInterview.Web.ViewModels.Interviews
{
    public class GetInterviewsVM
    {
        public IEnumerable<string> Nationality { get; set; }

        public IEnumerable<string> LocationType { get; set; }

        public IEnumerable<string> RankQuestion { get; set; }
    }
}
