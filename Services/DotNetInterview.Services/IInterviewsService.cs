namespace DotNetInterview.Services
{
    using DotNetInterview.Web.ViewModels.Interviews;
    using System.Threading.Tasks;

    public interface IInterviewsService
    {
        AllInterviewsVM All(int seniority);

        CreateInterviewVM CreateGetVM();

        Task Create(CreateInterviewVM model, string userId, string filePath);
    }
}
