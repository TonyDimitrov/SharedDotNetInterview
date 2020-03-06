namespace DotNetInterview.Web.ViewModels.Interviews.DTO
{
    using System.Collections.Generic;

    public class AllInterviewsDTO
    {
        public IEnumerable<AllInterviewDTO> Interviews { get; set; }
    }
}
