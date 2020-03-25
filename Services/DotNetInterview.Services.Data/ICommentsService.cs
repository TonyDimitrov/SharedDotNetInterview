namespace DotNetInterview.Services.Data
{
    using System.Threading.Tasks;

    using DotNetInterview.Web.ViewModels.Comments.DTO;

    public interface ICommentsService
    {
        T AllComments<T>(string id);

        Task<T> AddComment<T>(AddCommentDTO interviewComment, string userId);

        Task<bool> Delete(string commentId, string currentUserId, bool isAdmin);
    }
}
