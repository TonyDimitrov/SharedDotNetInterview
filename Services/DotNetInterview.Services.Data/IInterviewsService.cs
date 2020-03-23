namespace DotNetInterview.Services.Data
{
    using System.Threading.Tasks;

    using DotNetInterview.Web.ViewModels.Comments.DTO;
    using DotNetInterview.Web.ViewModels.Interviews;
    using DotNetInterview.Web.ViewModels.Interviews.DTO;
    using DotNetInterview.Web.ViewModels.Likes;

    public interface IInterviewsService
    {
        T All<T>(int seniority);

        CreateInterviewVM CreateGetVM();

        Task Create(CreateInterviewVM model, string userId, string filePath, IFileService fileService);

        T Details<T>(string interviewId, string currentUserId, bool isAdmin);

        Task<EditInterviewDTO> EditGet(string interviewId);

        Task Edit(EditInterviewDTO interview, string userId, string fileDirectory, IFileService fileService);

        Task Delete(string interviewId);

        Task AddComment(AddCommentDTO comment, string userId);

        T AllComments<T>(string interviewId, string currentUserId, bool isAdmin);

        Task<LikeVM> Liked(string interviewId, string userId);
    }
}
