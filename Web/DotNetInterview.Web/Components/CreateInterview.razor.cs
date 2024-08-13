namespace DotNetInterview.Web.Components
{
    using System.Threading.Tasks;

    using DotNetInterview.Services.Data;
    using DotNetInterview.Web.ViewModels.Interviews;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;

    public partial class CreateInterview
    {
        private const string LocationInOffice = "InOffice";

        [Inject]
        public IPresentationService PresentationService { get; set; }

        [Parameter]
        public CreateInterviewVM VmModel { get; set; } = new CreateInterviewVM();

        public bool ShowLocationField { get; set; }

        private async Task HandleFormSubmit(EditContext editContext)
        {
            await Task.FromResult(() => { });

            var jobTitle = this.VmModel.PositionTitle;
            var seniort = this.VmModel.Seniority;
        }

        public void SaveNewQuestion(CreateInterviewQuestionVM question)
        {
            CreateInterviewQuestionVM question1 = new CreateInterviewQuestionVM();
            question1.Content = question.Content;
            question1.GivenAnswer = question.GivenAnswer;

            this.VmModel.Questions.Add(question1);

            question.Content = string.Empty;
            question.GivenAnswer = string.Empty;
            question.Interesting = 0;
            question.Difficult = 0;
            question.Unexpected = 0;
        }

        public void SaveEditedQuestion(int questionIndex)
        {
            var editedInterview = this.VmModel.Questions[questionIndex];
            editedInterview.Content = this.VmModel.Question.Content;
            editedInterview.GivenAnswer = this.VmModel.Question.GivenAnswer;
            this.VmModel.IsBeingEdited = false;
            this.VmModel.CurentQuestionIndex = -1;
            this.VmModel.Question.Content = string.Empty;
            this.VmModel.Question.GivenAnswer = string.Empty;

        }

        public void GetEditQuestion(int questionIndex)
        {
            var editedQuestion = this.VmModel.Questions[questionIndex];
            this.VmModel.Question.Content = editedQuestion.Content;
            this.VmModel.Question.GivenAnswer = editedQuestion.GivenAnswer;
            this.VmModel.CurentQuestionIndex = questionIndex;
            this.VmModel.IsBeingEdited = true;
        }

        private void HandleInputRadioChange(string radioValue) => this.ShowLocationField = radioValue == LocationInOffice;
    }
}
