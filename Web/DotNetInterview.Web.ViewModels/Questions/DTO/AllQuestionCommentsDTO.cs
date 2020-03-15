namespace DotNetInterview.Web.ViewModels.Questions.DTO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class AllQuestionCommentsDTO
    {
        public string QuestionId { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string UseId { get; set; }

        public string UserFName { get; set; }

        public string UserLName { get; set; }
    }
}
