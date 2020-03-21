namespace DotNetInterview.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using DotNetInterview.Common;
    using DotNetInterview.Data;
    using DotNetInterview.Data.Common.Repositories;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Data.Models.Enums;
    using DotNetInterview.Services.Data.Extensions;
    using DotNetInterview.Services.Data.Helpers;
    using DotNetInterview.Web.ViewModels.Comments;
    using DotNetInterview.Web.ViewModels.Comments.DTO;
    using DotNetInterview.Web.ViewModels.Enums;
    using DotNetInterview.Web.ViewModels.Interviews;
    using DotNetInterview.Web.ViewModels.Interviews.DTO;
    using DotNetInterview.Web.ViewModels.Likes;
    using DotNetInterview.Web.ViewModels.Questions;
    using DotNetInterview.Web.ViewModels.Questions.DTO;
    using Microsoft.EntityFrameworkCore;

    public class InterviewsService : IInterviewsService
    {
        private readonly ApplicationDbContext db;
        private readonly IDeletableEntityRepository<Interview> categoriesRepository;

        public InterviewsService(ApplicationDbContext db, IDeletableEntityRepository<Interview> categoriesRepository)
        {
            this.db = db;
            this.categoriesRepository = categoriesRepository;
        }

        public T All<T>(int seniority)
        {
            AllInterviewsDTO interviewsDto = new AllInterviewsDTO();

            if (seniority == 0)
            {
                interviewsDto.Interviews = this.db.Interviews
                    .Where(i => !i.IsDeleted)
                .OrderByDescending(i => i.CreatedOn)
                .Select(i => new AllInterviewDTO
                {
                    InterviewId = i.Id,
                    PositionTitle = i.PositionTitle,
                    SeniorityAsNumber = (int)i.Seniority,
                    Date = i.HeldOnDate.ToLocalTime()
                    .ToString("dd MMM yyyy HH:mm", CultureInfo.InvariantCulture),
                    Likes = i.Likes
                    .Where(l => l.IsLiked)
                    .Count(),
                    Questions = i.Questions.Count,
                    CreatorId = i.UserId,
                    CreatorFName = i.User.FirstName,
                    CreatorLName = i.User.LastName != null ? i.User.LastName : string.Empty,
                    CreatorAvatar = i.User.Image,
                })
                .ToList();
            }
            else
            {
                interviewsDto.Interviews = this.db.Interviews
               .Where(i => (int)(object)i.Seniority == seniority)
               .OrderByDescending(i => i.HeldOnDate)
               .Select(i => new AllInterviewDTO
               {
                   InterviewId = i.Id,
                   PositionTitle = i.PositionTitle,
                   SeniorityAsNumber = (int)i.Seniority,
                   Date = i.HeldOnDate.ToString(GlobalConstants.FormatDate),
                   Likes = i.Likes
                   .Where(l => l.IsLiked)
                   .Count(),
                   Questions = i.Questions.Count,
                   CreatorId = i.UserId,
                   CreatorFName = i.User.FirstName,
                   CreatorLName = i.User.LastName != null ? i.User.LastName : string.Empty,
                   CreatorAvatar = i.User.Image,
               })
               .ToList();
            }

            var interviewsVM = new AllInterviewsVM
            {
                Interviews = interviewsDto.Interviews
                .Select(i =>
                new AllInterviewVM
                {
                    InterviewId = i.InterviewId,
                    Seniority = this.SeniorityNameParser(i.SeniorityAsNumber),
                    PositionTitle = this.PositionTitleParser(i.PositionTitle),
                    Date = i.Date,
                    Likes = i.Likes,
                    Questions = i.Questions,
                    CreatorId = i.CreatorId,
                    CreatorName = i.CreatorFName.FullUserNameParser(i.CreatorLName),
                    CreatorAvatar = i.CreatorAvatar != null ? i.CreatorAvatar : GlobalConstants.DefaultAvatar,
                })
                .ToList(),
            };

            return (T)(object)interviewsVM;
        }

        public async Task Create(CreateInterviewVM model, string userId, string fileDirectory, IFileService fileService)
        {
            var questions = new List<Question>();

            foreach (var q in model.Questions)
            {
                var fileName = await fileService.SaveFile(q.FormFile, fileDirectory);

                var rankValue = Math.Max(q.Interesting, Math.Max(q.Unexpected, q.Difficult));

                questions.Add(new Question
                {
                    Content = q.Content,
                    GivenAnswer = q.GivenAnswer,
                    CreatedOn = DateTime.UtcNow,
                    RankType = (QuestionRankType)rankValue,
                    UrlTask = fileName,
                });
            }

            LocationType locationType;

            if (!Enum.TryParse<LocationType>(model.LocationType, out locationType))
            {
                throw new ArgumentException($"Location type value: '{model.LocationType}' is invalid!");
            }

            var interview = new Interview
            {
                Seniority = (PositionSeniority)model.Seniority,
                PositionTitle = model.PositionTitle,
                PositionDescription = model.PositionDescription,
                HeldOnDate = DateTime.UtcNow,
                CreatedOn = DateTime.UtcNow,
                CompanyNationality = model.CompanyNationality,
                Employees = (EmployeesSize)model.Employees,
                LocationType = locationType,
                HeldOnInterviewLocation = locationType == LocationType.InOffice ? model.InterviewLocation : null,
                UserId = userId,
            };

            foreach (var q in questions)
            {
                interview.Questions.Add(q);
            }

            this.db.Interviews.Add(interview);
            this.db.SaveChanges();
        }

        public CreateInterviewVM CreateGetVM()
        {
            return new CreateInterviewVM
            {
                Questions = new List<CreateInterviewQuestionVM> { new CreateInterviewQuestionVM() },
                Select = new GetCreateInterviewsVM(),
            };
        }

        public T Details<T>(string interviewId, string currentUserId, bool isAdmin)
        {
            var interviewDTO = this.categoriesRepository.All()
                .Where(i => i.Id == interviewId && !i.IsDeleted)
                .Select(i => new DetailsInterviewDTO
                {
                    InterviewId = i.Id,
                    UserId = i.UserId,
                    UserFName = i.User.FirstName,
                    UserLName = i.User.LastName,
                    Seniority = Enum.Parse<PositionSeniorityVM>(i.Seniority.ToString()),
                    PositionTitle = i.PositionTitle,
                    PositionDescription = i.PositionDescription,
                    CompanyNationality = i.CompanyNationality,
                    CompanySize = Enum.Parse<EmployeesSizeVM>(i.Employees.ToString()),
                    LocationType = Enum.Parse<LocationTypeVM>(i.LocationType.ToString()),
                    InterviewLocation = i.HeldOnInterviewLocation,
                    CreatedOn = i.CreatedOn,
                    ModifiedOn = i.ModifiedOn,
                    Likes = i.Likes
                    .Where(l => l.IsLiked)
                    .Count(),
                    IsLiked = i.Likes
                    .FirstOrDefault(l => l.UserId == currentUserId && l.IsLiked) != null ? true : false,
                    InterviewQns = i.Questions
                        .Where(q => !q.IsDeleted)
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
                        .ToList(),
                    InterviewComments = i.Comments
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
                .FirstOrDefault();

            var interviewVM = new DetailsInterviewVM
            {
                InterviewId = interviewDTO.InterviewId,
                UserId = interviewDTO.UserId,
                UserFullName = interviewDTO.UserFName.FullUserNameParser(interviewDTO.UserLName),
                Seniority = Helper.ParseEnum<PositionSeniorityVM>(interviewDTO.Seniority),
                PositionTitle = interviewDTO.PositionTitle,
                PositionDescription = interviewDTO.PositionDescription == null ? GlobalConstants.NoDescription : interviewDTO.PositionDescription,
                CompanyNationality = interviewDTO.CompanyNationality,
                CompanySize = Helper.ParseEnum<EmployeesSizeVM>(interviewDTO.CompanySize),
                LocationType = Helper.ParseEnum<LocationTypeVM>(interviewDTO.LocationType),
                InterviewLocation = interviewDTO.InterviewLocation,
                CreatedOn = interviewDTO.CreatedOn.ToString(GlobalConstants.FormatDate),
                ModifiedOn = interviewDTO.ModifiedOn?.ToString(GlobalConstants.FormatDate),
                HideAddCommentForm = Utils.HideAddComment(currentUserId),
                Likes = interviewDTO.Likes,
                AddLike = interviewDTO.IsLiked ? GlobalConstants.LikedCss : string.Empty,
                InterviewQns = interviewDTO.InterviewQns
                    .Select(q => new AllInterviewQuestionsVM
                    {
                        QuestionId = q.QuestionId,
                        Content = q.Content,
                        Answer = q.Answer,
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
                InterviewComments = interviewDTO.InterviewComments
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
                    })
                    .ToList(),
            };

            return (T)(object)interviewVM;
        }

        public async Task AddComment(AddCommentDTO postComment, string userId)
        {
            var interview = await this.categoriesRepository.GetByIdWithDeletedAsync(postComment.Id);

            var comment = new Comment
            {
                InterviewId = interview.Id,
                Content = postComment.Content,
                CreatedOn = DateTime.UtcNow,
                UserId = userId,
            };

            interview.Comments.Add(comment);
            await this.categoriesRepository.SaveChangesAsync();
        }

        public T AllComments<T>(string interviewId, string currentUserId, bool isAdmin)
        {
            var commentsDTO = this.categoriesRepository.All()
               .Where(i => i.Id == interviewId)
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
                   ParentId = interviewId,
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
                  CommentId = c.CommentId,
                  HideDelete = Utils.HideDelete(c.UserId, currentUserId, isAdmin),
                  HideAdd = Utils.HideAddComment(currentUserId),
                  ParentId = interviewId,
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

        public async Task<LikeVM> Liked(string interviewId, string userId)
        {
            var liked = this.categoriesRepository.All()
                 .Where(i => i.Id == interviewId)
                 .SelectMany(i => i.Likes)
                 .FirstOrDefault(l => l.UserId == userId);

            bool isLiked = false;

            if (liked == null)
            {
                var interview = this.categoriesRepository.All()
              .FirstOrDefault(i => i.Id == interviewId);

                var like = new Like
                {
                    InterviewId = interviewId,
                    UserId = userId,
                    CreatedOn = DateTime.UtcNow,
                    IsLiked = true,
                };

                interview.Likes.Add(like);
                await this.categoriesRepository.SaveChangesAsync();

                isLiked = true;
            }
            else if (!liked.IsLiked)
            {
                liked.IsLiked = true;
                await this.categoriesRepository.SaveChangesAsync();

                isLiked = true;
            }
            else if (liked.IsLiked)
            {
                liked.IsLiked = false;
                await this.categoriesRepository.SaveChangesAsync();

                isLiked = false;
            }

            var count = this.categoriesRepository.All()
                .Where(i => i.Id == interviewId)
                .SelectMany(i => i.Likes)
                .Where(l => l.IsLiked).Count();

            return new LikeVM
            {
                Count = count,
                LikedCss = isLiked ? GlobalConstants.LikedCss : string.Empty,
            };
        }

        private string SeniorityNameParser(int seniority) =>
            seniority switch
            {
                0 => "All",
                1 => "Junior developer",
                2 => "Regular developer",
                3 => "Senior developer",
                4 => "Lead developer",
                5 => "Architect",
                99 => "Other",
                _ => throw new ArgumentException($"Seniority type [{seniority}] is invalid!"),
            };

        private string PositionTitleParser(string positionTitle)
        {
            if (positionTitle != null && positionTitle.Length <= 50)
            {
                return positionTitle;
            }
            else
            {
                return positionTitle.Substring(0, 47) + "...";
            }
        }
    }
}
