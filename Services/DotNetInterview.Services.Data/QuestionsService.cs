namespace DotNetInterview.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using DotNetInterview.Data.Common.Repositories;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Services.Data.Extensions;
    using DotNetInterview.Web.ViewModels.Comments;
    using DotNetInterview.Web.ViewModels.Comments.DTO;
    using Microsoft.EntityFrameworkCore;

    public class QuestionsService : IQuestionsService
    {
        private readonly IDeletableEntityRepository<Question> questionRepository;

        public QuestionsService(IDeletableEntityRepository<Question> questionRepository)
        {
            this.questionRepository = questionRepository;
        }

        public async Task AddComment(AddCommentDTO postComment, string userId)
        {
            var question = await this.questionRepository.GetByIdWithDeletedAsync(postComment.Id);

            var comment = new Comment
            {
                QuestionId = question.Id,
                Content = postComment.Content,
                CreatedOn = DateTime.UtcNow,
                UserId = userId,
            };

            question.Comments.Add(comment);
            await this.questionRepository.SaveChangesAsync();
        }

        public T AllComments<T>(string id)
        {
            var commentsDTO = this.questionRepository.All()
               .Where(i => i.Id == id)
               .Include(i => i.Comments)
               .ThenInclude(c => c.User)
               .FirstOrDefault()
               .Comments
               .Where(c => !c.IsDeleted)
               .Select(c => new AllCommentsDTO
               {
                   Id = id,
                   Content = c.Content,
                   CreatedOn = c.CreatedOn,
                   ModifiedOn = c.ModifiedOn,
                   UserId = c.UserId,
                   UserFName = c.User.FirstName,
                   UserLName = c.User.LastName,
               })
               .OrderBy(c => c.CreatedOn)
               .ToList();

            var commentsVM = commentsDTO
              .Select(c => new AllCommentsVM
              {
                  Id = id,
                  Content = c.Content,
                  CreatedOn = c.CreatedOn.DateTimeViewFormater(),
                  HasBeenModified = c.ModifiedOn != null && c.ModifiedOn != c.CreatedOn ? true : false,
                  ModifiedOn = c.ModifiedOn?.DateTimeViewFormater(),
                  UserId = c.UserId,
                  UserFullName = c.UserFName.FullUserNameParser(c.UserLName),
              })
              .ToList();

            return (T)(object)commentsVM;
        }
    }
}