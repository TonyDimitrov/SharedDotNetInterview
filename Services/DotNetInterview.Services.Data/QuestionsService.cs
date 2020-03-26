namespace DotNetInterview.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using DotNetInterview.Common;
    using DotNetInterview.Data.Common.Repositories;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Services.Data.Extensions;
    using DotNetInterview.Services.Data.Helpers;
    using DotNetInterview.Web.ViewModels.Comments;
    using DotNetInterview.Web.ViewModels.Comments.DTO;
    using DotNetInterview.Web.ViewModels.Enums;
    using DotNetInterview.Web.ViewModels.Interviews;
    using DotNetInterview.Web.ViewModels.Interviews.DTO;
    using DotNetInterview.Web.ViewModels.Questions;
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

        public AllIQuestionsVM All(int rank, string currentUserId, bool isAdmin)
        {
            bool all = rank > 3 ? true : false;

            var questionsDTO = this.questionRepository.All()
                .Where(q => (int)(object)q.RankType == rank || all)
                        .Select(q => new AllInterviewQuestionsDTO
                        {
                            QuestionId = q.Id,
                            Content = q.Content,
                            Answer = q.GivenAnswer,
                            CreatedOn = q.CreatedOn,
                            ModifiedOn = q.ModifiedOn,
                            Ranked = Enum.Parse<QuestionRankTypeVM>(q.RankType.ToString()),
                            File = q.UrlTask,
                            InterviewId = q.InterviewId,
                            QnsComments = q.Comments
                            .Where(q => !q.IsDeleted)
                            .Select(c => new AllCommentsDTO
                            {
                                CommentId = c.Id,
                                HideDelete = Utils.HideDelete(c.UserId, currentUserId, isAdmin),
                                HideAdd = Utils.HideAddComment(currentUserId),
                                Content = c.Content,
                                CreatedOn = c.CreatedOn,
                                ModifiedOn = c.ModifiedOn,
                                UserId = c.UserId,
                                UserFName = c.User.FirstName,
                                UserLName = c.User.LastName,
                            })
                            .OrderBy(c => c.CreatedOn)
                            .ToList(),
                        })
                        .ToList();

            var questions = new AllIQuestionsVM
            {
                HideAddComment = Utils.HideAddComment(currentUserId),
                Questions = questionsDTO.Select(q => new AllInterviewQuestionsVM
                {
                    QuestionId = q.QuestionId,
                    Content = q.Content.SanitizeTextInput(),
                    Answer = q.Answer.SanitizeTextInput(),
                    HideAnswer = q.Answer == null,
                    CreatedOn = q.CreatedOn.DateTimeViewFormater(),
                    ModifiedOn = q.ModifiedOn?.DateTimeViewFormater(),
                    HideRanked = q.Ranked == 0,
                    Ranked = Helper.ParseEnum<QuestionRankTypeVM>(q.Ranked),
                    HideFile = q.File == null,
                    File = q.File,
                    InterviewId = q.InterviewId,
                    QnsComments = q.QnsComments
                        .Select(c => new AllCommentsVM
                        {
                            CommentId = c.CommentId,
                            HideDelete = Utils.HideDelete(c.UserId, currentUserId, isAdmin),
                            HideAdd = Utils.HideAddComment(currentUserId),
                            Content = c.Content,
                            CreatedOn = c.CreatedOn.DateTimeViewFormater(),
                            ModifiedOn = c.ModifiedOn?.DateTimeViewFormater(),
                            HasBeenModified = Utils.IsModified(c.CreatedOn, c.ModifiedOn),
                            UserId = c.UserId,
                            UserFullName = c.UserFName.FullUserNameParser(c.UserLName),
                        }),
                }),
            };

            return questions;
        }

        public T AllComments<T>(string id, string currentUserId, bool isAdmin)
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
                   CommentId = c.Id,
                   HideDelete = Utils.HideDelete(c.UserId, currentUserId, isAdmin),
                   HideAdd = Utils.HideAddComment(currentUserId),
                   ParentId = id,
                   Content = c.Content,
                   CreatedOn = c.CreatedOn,
                   ModifiedOn = c.ModifiedOn,
                   UserId = c.UserId,
                   UserFName = c.User.FirstName,
                   UserLName = c.User.LastName,
               })
               .OrderBy(c => c.ModifiedOn)
               .ToList();

            var commentsVM = commentsDTO
              .Select(c => new AllCommentsVM
              {
                  CommentId = c.CommentId,
                  HideDelete = Utils.HideDelete(c.UserId, currentUserId, isAdmin),
                  HideAdd = Utils.HideAddComment(currentUserId),
                  ParentId = id,
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

        public Task<bool> Delete(string commentId)
        {
            throw new NotImplementedException();
        }
    }
}