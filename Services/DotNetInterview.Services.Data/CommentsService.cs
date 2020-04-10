namespace DotNetInterview.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using DotNetInterview.Data.Common.Repositories;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Web.ViewModels.Comments.DTO;

    public class CommentsService : ICommentsService
    {
        private readonly IDeletableEntityRepository<Comment> commentsRepository;

        public CommentsService(IDeletableEntityRepository<Comment> commentsRepository)
        {
            this.commentsRepository = commentsRepository;
        }

        public async Task<bool> Delete(string commentId, string currentUserId, bool isAdmin)
        {
            var comment = await this.commentsRepository.GetByIdWithDeletedAsync(commentId);

            if (isAdmin || comment.UserId == currentUserId)
            {
                this.commentsRepository.Delete(comment);
            }

            return (await this.commentsRepository.SaveChangesAsync()) != 0 ? true : false;
        }
    }
}
