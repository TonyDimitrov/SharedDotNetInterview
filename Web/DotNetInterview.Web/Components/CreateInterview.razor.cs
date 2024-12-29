namespace DotNetInterview.Web.Components
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using DotNetInterview.Services.Data;
    using DotNetInterview.Web.ViewModels.Enums;
    using DotNetInterview.Web.ViewModels.Interviews;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Authorization;
    using Microsoft.AspNetCore.Components.Forms;

    public partial class CreateInterview
    {
        private const string LocationInOffice = "InOffice";

        [Inject]
        private IPresentationService PresentationService { get; set; }

        [Inject]
        private IInterviewsService interviewsService { get; set; }

        [Inject]
        private IFileService fileService { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Parameter]
        public CreateInterviewVM VmModel { get; set; } = new CreateInterviewVM();

        public bool ShowLocationField { get; set; }

        private async Task HandleFormSubmit(EditContext editContext)
        {
            await Task.FromResult(() => { });

            var jobTitle = this.VmModel.PositionTitle;
            var seniort = this.VmModel.Seniority;

            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await interviewsService.Create(VmModel, userId, null, fileService);
        }

        public void SaveNewQuestion(List<CreateInterviewQuestionVM> questions, CreateInterviewQuestionVM question)
        {
            CreateInterviewQuestionVM question1 = new CreateInterviewQuestionVM();
            question1.Content = question.Content;
            question1.GivenAnswer = question.GivenAnswer;

            questions.Add(question1);

            question.Content = string.Empty;
            question.GivenAnswer = string.Empty;
            question.Interesting = 0;
            question.Difficult = 0;
            question.Unexpected = 0;
        }

        public void SaveEditedQuestion(int questionIndex, QuestionGeneralTypeVM questionType)
        {
            if (questionType == QuestionGeneralTypeVM.CultyralFit)
            {
                var editedInterview = this.VmModel.CultFitQuestions[questionIndex];
                editedInterview.Content = this.VmModel.CultFitQuestion.Content;
                editedInterview.GivenAnswer = this.VmModel.CultFitQuestion.GivenAnswer;
                editedInterview.IsInteresting = this.VmModel.CultFitQuestion.IsInteresting;
                editedInterview.IsUnexpected = this.VmModel.CultFitQuestion.IsUnexpected;
                editedInterview.IsDifficult = this.VmModel.CultFitQuestion.IsDifficult;
                this.VmModel.IsBeingEdited = false;
                this.VmModel.CurrentCultFitQuestionIndex = -1;
                this.VmModel.CultFitQuestion.Content = string.Empty;
                this.VmModel.CultFitQuestion.GivenAnswer = string.Empty;
            }
            else if (questionType == QuestionGeneralTypeVM.Technical)
            {
                var editedInterview = this.VmModel.TechnicalQuestions[questionIndex];
                editedInterview.Content = this.VmModel.TechnicalQuestion.Content;
                editedInterview.GivenAnswer = this.VmModel.TechnicalQuestion.GivenAnswer;
                editedInterview.IsInteresting = this.VmModel.TechnicalQuestion.IsInteresting;
                editedInterview.IsUnexpected = this.VmModel.TechnicalQuestion.IsUnexpected;
                editedInterview.IsDifficult = this.VmModel.TechnicalQuestion.IsDifficult;
                this.VmModel.IsBeingEdited = false;
                this.VmModel.CurrentTechQuestionIndex = -1;
                this.VmModel.TechnicalQuestion.Content = string.Empty;
                this.VmModel.TechnicalQuestion.GivenAnswer = string.Empty;
            }
        }

        private void DeleteQuestion(int index)
        { 
        
        }

        public void GetEditCultFitQuestion(int questionIndex)
        {
            var editedQuestion = this.VmModel.CultFitQuestions[questionIndex];
            this.VmModel.CultFitQuestion.Content = editedQuestion.Content;
            this.VmModel.CultFitQuestion.GivenAnswer = editedQuestion.GivenAnswer;
            this.VmModel.CurrentCultFitQuestionIndex = questionIndex;
            this.VmModel.CultFitQuestion.IsInteresting = editedQuestion.IsInteresting;
            this.VmModel.CultFitQuestion.IsUnexpected = editedQuestion.IsUnexpected;
            this.VmModel.CultFitQuestion.IsDifficult = editedQuestion.IsDifficult;
            this.VmModel.IsBeingEdited = true;
        }

        public void GetEditTechtQuestion(int questionIndex)
        {
            var editedQuestion = this.VmModel.TechnicalQuestions[questionIndex];
            this.VmModel.TechnicalQuestion.Content = editedQuestion.Content;
            this.VmModel.TechnicalQuestion.GivenAnswer = editedQuestion.GivenAnswer;
            this.VmModel.CurrentTechQuestionIndex = questionIndex;
            this.VmModel.IsBeingEdited = true;
        }

        private void MoveToCultFitQuestions()
        {
            VmModel.ShowGeneralQuestions = false;
            VmModel.ShowCultFitQuestions = true;
            VmModel.ShowTechQuestions = false;
        }

        private void BackToGeneralQuestions()
        {
            VmModel.ShowGeneralQuestions = true;
            VmModel.ShowCultFitQuestions = false;
            VmModel.ShowTechQuestions = false;
        }

        private void BackToCultFitQuestions()
        {
            VmModel.ShowGeneralQuestions = false;
            VmModel.ShowCultFitQuestions = true;
            VmModel.ShowTechQuestions = false;
        }

        private void MoveToTechQuestions()
        {
            VmModel.ShowCultFitQuestions = false;
            VmModel.ShowTechQuestions = true;
        }

        private void FinalSubmit()
        {
            VmModel.ShowCultFitQuestions = true;
        }

        private void HandleInputRadioChange(string radioValue) => this.ShowLocationField = radioValue == LocationInOffice;
    }
}
