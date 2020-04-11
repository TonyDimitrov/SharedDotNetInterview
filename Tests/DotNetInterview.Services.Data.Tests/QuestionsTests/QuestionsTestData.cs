namespace DotNetInterview.Services.Data.Tests.QuestionsTests
{
    using System;
    using System.Collections.Generic;

    using DotNetInterview.Web.ViewModels.Interviews;
    using DotNetInterview.Web.ViewModels.Questions;

    public class QuestionsTestData
    {
        public AllIQuestionsVM GetQuestions()
        {
            var questions = new List<AllInterviewQuestionsVM>
            {
                new AllInterviewQuestionsVM
                {
                    Content = "content 1",
                    Answer = "answer 2",
                    CreatedOn = DateTime.UtcNow.ToString(),
                    Ranked = "Most interesting",
                },
            };

            return new AllIQuestionsVM
            {
                Questions = questions,
            };
        }
    }
}
