namespace DotNetInterview.Web.ViewModels.Questions.DTO
{
    using System.Collections.Generic;

    using DotNetInterview.Web.ViewModels.Interviews.DTO;

    public class AllQuestionsDTO
    {
        public AllQuestionsDTO()
        {
            this.Questions = new List<AllInterviewQuestionsDTO>();
        }

        public IEnumerable<AllInterviewQuestionsDTO> Questions { get; set; }
    }
}
