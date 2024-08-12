namespace DotNetInterview.Web.Components
{
    using DotNetInterview.Web.ViewModels.Interviews;
    using Microsoft.AspNetCore.Components;

    public partial class Questions
    {
        [Parameter]
        public CreateInterviewQuestionVM Question { get; set; }
    }
}
