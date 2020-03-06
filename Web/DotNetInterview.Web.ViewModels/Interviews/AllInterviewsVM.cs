using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetInterview.Web.ViewModels.Interviews
{
   public class AllInterviewsVM
    {
        public IEnumerable<AllInterviewVM> Interviews { get; set; }
    }
}
