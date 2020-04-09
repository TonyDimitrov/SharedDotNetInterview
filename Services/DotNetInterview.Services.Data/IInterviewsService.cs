namespace DotNetInterview.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DotNetInterview.Web.ViewModels.Comments.DTO;
    using DotNetInterview.Web.ViewModels.Interviews;
    using DotNetInterview.Web.ViewModels.Interviews.DTO;
    using DotNetInterview.Web.ViewModels.Likes;

    public interface IInterviewsService
    {
        Task<AllInterviewsVM> All(int seniority);

        AllInterviewsVM AllByPage(int page, AllInterviewsVM interviewVM, IEnumerable<InterviewVM> interviews);

        CreateInterviewVM CreateGetVM();

        Task Create(CreateInterviewVM model, string userId, string filePath, IFileService fileService);

        DetailsInterviewVM Details(string interviewId, string currentUserId, bool isAdmin);

        Task<EditInterviewDTO> EditGet(string interviewId);

        Task Edit(EditInterviewDTO interview, string userId, string fileDirectory, IFileService fileService);

        Task Delete(string interviewId, string currentUserId, bool isAdmin);

        Task HardDelete(string interviewId, bool isAdmin);

        Task AddComment(AddCommentDTO comment, string userId);

        T AllComments<T>(string interviewId, string currentUserId, bool isAdmin);

        Task<LikeVM> Liked(string interviewId, string userId);
    }
}
