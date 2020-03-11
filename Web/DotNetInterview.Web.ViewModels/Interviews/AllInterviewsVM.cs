namespace DotNetInterview.Web.ViewModels.Interviews
{
    using System;
    using System.Collections.Generic;
    using System.Text;

   public class AllInterviewsVM
    {
        public IEnumerable<AllInterviewVM> Interviews { get; set; }
    }
}
