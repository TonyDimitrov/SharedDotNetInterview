namespace DotNetInterview.Services.Data
{
    using System.Threading.Tasks;

    using DotNetInterview.Web.ViewModels.Interviews.DTO;

    public interface ICommentsService
    {
        T AllComments<T>(string id);

        Task<T> AddComment<T>(AddCommentDTO interviewComment, string userId);
    }
}
