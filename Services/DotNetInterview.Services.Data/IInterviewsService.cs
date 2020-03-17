namespace DotNetInterview.Services.Data
{
    using System.Threading.Tasks;

    using DotNetInterview.Web.ViewModels.Comments.DTO;
    using DotNetInterview.Web.ViewModels.Interviews;

    public interface IInterviewsService
    {
        T All<T>(int seniority);

        CreateInterviewVM CreateGetVM();

        Task Create(CreateInterviewVM model, string userId, string filePath, IFileService fileService);

        T Details<T>(string interviewId, string currentUserId, bool isAdmin);

        Task AddComment(AddCommentDTO comment, string userId);

        T AllComments<T>(string interviewId, string currentUserId, bool isAdmin);
    }
}
