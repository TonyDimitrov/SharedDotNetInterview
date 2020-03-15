namespace DotNetInterview.Services.Data
{
    using DotNetInterview.Web.ViewModels.Comments.DTO;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IQuestionsService
    {
        T AllComments<T>(string id);

        Task AddComment(AddCommentDTO interviewComment, string userId);
    }
}
