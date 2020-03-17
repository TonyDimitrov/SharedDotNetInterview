namespace DotNetInterview.Services.Data
{
    using System.Threading.Tasks;

    using DotNetInterview.Web.ViewModels.Comments.DTO;

    public interface IQuestionsService
    {
        Task<bool> Delete(string commentId);

        T AllComments<T>(string id);

        Task AddComment(AddCommentDTO interviewComment, string userId);
    }
}
