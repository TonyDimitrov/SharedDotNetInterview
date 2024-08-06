namespace DotNetInterview.Web.Components
{
    using System.Threading.Tasks;

    using DotNetInterview.Web.ViewModels.Interviews;
    using Microsoft.AspNetCore.Components.Forms;

    public partial class CreateInterview
    {
        public CreateInterviewVM CreateInterviewVM { get; set; } = new CreateInterviewVM();

        private async Task HandleFormSubmit(EditContext editContext)
        {
            await Task.FromResult(() => {
                var num = 5 + 5;
            });

            var jobTitle = this.CreateInterviewVM.PositionTitle;
        }
    }
}
