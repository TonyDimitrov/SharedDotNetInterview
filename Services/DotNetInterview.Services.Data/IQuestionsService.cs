namespace DotNetInterview.Services.Data
{
    using System.Threading.Tasks;

    using DotNetInterview.Web.ViewModels.Comments.DTO;
    using DotNetInterview.Web.ViewModels.Questions;

    public interface IQuestionsService
    {
        AllIQuestionsVM All(int rank, string currentUserId, bool isAdmin);

        Task<bool> Delete(string commentId);

        T AllComments<T>(string id, string currentUserId, bool isAdmin);

        Task AddComment(AddCommentDTO interviewComment, string userId);
    }
}
