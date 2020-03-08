namespace DotNetInterview.Services
{
    using System.Threading.Tasks;

    using DotNetInterview.Web.ViewModels.Interviews;

    public interface IInterviewsService
    {
        T All<T>(int seniority);

        CreateInterviewVM CreateGetVM();

        Task Create(CreateInterviewVM model, string userId, string filePath, IFileService fileService);
    }
}
