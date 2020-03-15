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

        public async Task<T> AddComment<T>(AddCommentDTO interviewComment, string userId)
        {
            return (T)(object) await this.commentsRepository.GetByIdWithDeletedAsync(interviewComment.Id);

            //var comment = new Comment
            //{
            //    InterviewId = interview.Id,
            //    Content = interviewComment.Content,
            //    CreatedOn = DateTime.UtcNow,
            //    UserId = userId,
            //};

            //interview.Comments.Add(comment);
            //await this.categoriesRepository.SaveChangesAsync();
        }

        public T AllComments<T>(string id)
        {
            throw new NotImplementedException();
        }
    }
}
