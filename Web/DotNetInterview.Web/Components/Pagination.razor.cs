namespace DotNetInterview.Web.Components
{
    using System.Threading.Tasks;

    using DotNetInterview.Web.ViewModels.Interviews;
    using Microsoft.AspNetCore.Components;

    public partial class Pagination
    {
        [Parameter]
        public CreateInterviewVM Interview { get; set; } = new CreateInterviewVM();

        [Parameter]
        public EventCallback<int> OnChange { get; set; }

        public async Task EditQuestion(int questionIndex)
        {
            //var editedQuestion = this.Interview.Questions[questionIndex];
            //this.Interview.Question.Content = editedQuestion.Content;
            //this.Interview.Question.GivenAnswer = editedQuestion.GivenAnswer;

            await this.OnChange.InvokeAsync(questionIndex);
        }
    }
}
