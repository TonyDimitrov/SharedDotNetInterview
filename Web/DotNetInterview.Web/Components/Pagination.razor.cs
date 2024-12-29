namespace DotNetInterview.Web.Components
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DotNetInterview.Web.ViewModels.Interviews;
    using Microsoft.AspNetCore.Components;

    public partial class Pagination
    {
        [Parameter]
        public List<CreateInterviewQuestionVM> Questions { get; set; } = new List<CreateInterviewQuestionVM>();

        [Parameter]
        public CreateInterviewQuestionVM Question { get; set; }

        [Parameter]
        public int CurrentQuestionIndex { get; set; }

        [Parameter]
        public EventCallback<int> OnChange { get; set; }

        public async Task EditQuestion(int questionIndex)
        {
            var editedQuestion = this.Questions[questionIndex];
            this.Question.Content = editedQuestion.Content;
            this.Question.GivenAnswer = editedQuestion.GivenAnswer;

            await this.OnChange.InvokeAsync(questionIndex);
        }
    }
}
