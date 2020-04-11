namespace DotNetInterview.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DotNetInterview.Web.ViewModels.Comments.DTO;
    using DotNetInterview.Web.ViewModels.Interviews;
    using DotNetInterview.Web.ViewModels.Questions;

    public interface IQuestionsService
    {
        AllIQuestionsVM All(int rank, string currentUserId, bool isAdmin);

        AllIQuestionsVM AllByPage(int page, AllIQuestionsVM questionsVM, IEnumerable<AllInterviewQuestionsVM> collection);

        T AllComments<T>(string id, string currentUserId, bool isAdmin);

        Task AddComment(AddCommentDTO interviewComment, string userId);
    }
}
