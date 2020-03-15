﻿namespace DotNetInterview.Web.ViewModels.Interviews
{
    using System.Collections.Generic;

    using DotNetInterview.Web.ViewModels.Questions;

    public class AllInterviewQuestionsVM
    {
        public AllInterviewQuestionsVM() => this.QnsComments = new List<AllQuestionCommentsVM>();

        public string QuestionId { get; set; }

        public string Content { get; set; }

        public bool HideAnswer { get; set; }

        public string Answer { get; set; }

        public bool HideRanked { get; set; }

        public string Ranked { get; set; }

        public string CreatedOn { get; set; }

        public string ModifiedOn { get; set; }

        public bool HideFile { get; set; }

        public string File { get; set; }

        public IEnumerable<AllQuestionCommentsVM> QnsComments { get; set; }
    }
}
