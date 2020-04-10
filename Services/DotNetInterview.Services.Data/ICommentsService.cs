namespace DotNetInterview.Services.Data
{
    using System.Threading.Tasks;

    using DotNetInterview.Web.ViewModels.Comments.DTO;

    public interface ICommentsService
    {
        Task<bool> Delete(string commentId, string currentUserId, bool isAdmin);
    }
}
