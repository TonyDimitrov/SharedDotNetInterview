namespace DotNetInterview.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using DotNetInterview.Common;
    using DotNetInterview.Data.Common.Repositories;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Services.Data.Extensions;
    using DotNetInterview.Services.Data.Helpers;
    using DotNetInterview.Web.Infrastructure.Extensions;
    using DotNetInterview.Web.ViewModels.Comments;
    using DotNetInterview.Web.ViewModels.Comments.DTO;
    using DotNetInterview.Web.ViewModels.Enums;
    using DotNetInterview.Web.ViewModels.Interviews;
    using DotNetInterview.Web.ViewModels.Interviews.DTO;
    using DotNetInterview.Web.ViewModels.Questions;

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
                        .OrderByDescending(q => q.CreatedOn)
                        .Select(q => new AllInterviewQuestionsDTO
                        {
                            QuestionId = q.Id,
                            Content = q.Content,
                            Answer = q.GivenAnswer,
                            CreatedOn = q.CreatedOn,
                            ModifiedOn = q.ModifiedOn,
                            Ranked = q.RankType.ToString(),
                            RankImgType = (QuestionRankImgType)(int)q.RankType,
                            File = q.UrlTask,
                            InterviewId = q.InterviewId,
                            QnsComments = q.Comments
                            .Where(q => !q.IsDeleted)
                            .Select(c => new AllCommentsDTO
                            {
                                CommentId = c.Id,
                                Content = c.Content,
                                CreatedOn = c.CreatedOn,
                                ModifiedOn = c.ModifiedOn,
                                UserId = c.UserId,
                                UserFName = c.User.FirstName,
                                UserLName = c.User.LastName,
                            })
                            .OrderBy(c => c.CreatedOn),
                        });

            var questions = new AllIQuestionsVM(rank, Utils.HideAddComment(currentUserId))
            {
                Questions = questionsDTO
                .Select(q => new AllInterviewQuestionsVM
                {
                    QuestionId = q.QuestionId,
                    Content = q.Content.SanitizeTextInput(),
                    Answer = q.Answer.SanitizeTextInput(),
                    HideAnswer = q.Answer == null,
                    CreatedOn = q.CreatedOn.ToLocalTime().ToString(GlobalConstants.FormatDate, CultureInfo.InvariantCulture),
                    ModifiedOn = q.ModifiedOn != null ? q.ModifiedOn.Value.ToLocalTime()
                    .ToString(GlobalConstants.FormatDate, CultureInfo.InvariantCulture) : null,
                    HideRanked = q.Ranked == GlobalConstants.None,
                    RankImgName = q.RankImgType.DisplayName(),
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
                            CreatedOn = c.CreatedOn.ToLocalTime().ToString(GlobalConstants.FormatDate, CultureInfo.InvariantCulture),
                            ModifiedOn = c.ModifiedOn != null ? q.ModifiedOn.Value.ToLocalTime()
                            .ToString(GlobalConstants.FormatDate, CultureInfo.InvariantCulture) : null,
                            HasBeenModified = Utils.IsModified(c.CreatedOn, c.ModifiedOn),
                            UserId = c.UserId,
                            UserFullName = c.UserFName.FullUserNameParser(c.UserLName),
                        }),
                })
                .ToList(),
            };

            return questions;
        }

        public AllIQuestionsVM AllByPage(int page, AllIQuestionsVM questionsVM, IEnumerable<AllInterviewQuestionsVM> collection)
        {
            questionsVM.Questions = questionsVM.SetPagination<AllInterviewQuestionsVM>(collection, page);
            questionsVM.QuestionRanks = Utils.AllQuestionRanks();

            return questionsVM;
        }

        public T AllComments<T>(string id, string currentUserId, bool isAdmin)
        {
            var commentsDTO = this.questionRepository.All()
               .Where(i => i.Id == id)
               .Select(i => i.Comments
               .Select(c => new AllCommentsDTO
               {
                   CommentId = c.Id,
                   ParentId = id,
                   Content = c.Content,
                   CreatedOn = c.CreatedOn,
                   ModifiedOn = c.ModifiedOn,
                   UserId = c.UserId,
                   UserFName = c.User.FirstName,
                   UserLName = c.User.LastName,
               })
               .OrderBy(c => c.ModifiedOn)
               .ToList())
               .FirstOrDefault();

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
    }
}
