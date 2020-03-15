namespace DotNetInterview.Services.Data
{
    using System.Threading.Tasks;

    using DotNetInterview.Web.ViewModels.Interviews;
    using DotNetInterview.Web.ViewModels.Interviews.DTO;

    public interface IInterviewsService
    {
        T All<T>(int seniority);

        CreateInterviewVM CreateGetVM();

        Task Create(CreateInterviewVM model, string userId, string filePath, IFileService fileService);

        T Details<T>(string interviewId);

        Task AddComment(AddCommentDTO comment, string userId);

        T AllComments<T>(string interviewId);
    }
}
