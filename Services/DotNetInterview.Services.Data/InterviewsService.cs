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
    using Microsoft.EntityFrameworkCore;

    public class InterviewsService : IInterviewsService
    {
        private readonly ApplicationDbContext db;
        private readonly IDeletableEntityRepository<Interview> interviewsRepository;
        private readonly IDeletableEntityRepository<Question> questionsEntityRepository;
        private readonly IDeletableEntityRepository<Comment> commentsEntityRepository;
        private readonly IDeletableEntityRepository<Like> likesEntityRepository;
        private readonly IImporterHelperService importerHelperService;

        public InterviewsService(
            ApplicationDbContext db,
            IDeletableEntityRepository<Interview> categoriesRepository,
            IDeletableEntityRepository<Question> questionsEntityRepository,
            IDeletableEntityRepository<Comment> commentsEntityRepository,
            IDeletableEntityRepository<Like> likesEntityRepository,
            IImporterHelperService importerHelperService)
        {
            this.db = db;
            this.interviewsRepository = categoriesRepository;
            this.questionsEntityRepository = questionsEntityRepository;
            this.commentsEntityRepository = commentsEntityRepository;
            this.likesEntityRepository = likesEntityRepository;
            this.importerHelperService = importerHelperService;
        }

        public async Task<AllInterviewsVM> All(int seniority, int pageIndex)
        {
            var selectAllSeniorities = seniority == 0;

            var interviewsDto = await Task.Run(() =>
            {
                return this.interviewsRepository.All()
                .Where(i => (int)(object)i.Seniority == seniority || selectAllSeniorities)
                .OrderByDescending(i => i.HeldOnDate)
                .Select(i => new AllInterviewDTO
                {
                    InterviewId = i.Id,
                    PositionTitle = i.PositionTitle,
                    SeniorityAsNumber = (int)i.Seniority,
                    Date = i.HeldOnDate.ToString(GlobalConstants.FormatDate, CultureInfo.InvariantCulture),
                    Likes = i.Likes
                    .Where(l => l.IsLiked)
                    .Count(),
                    Questions = i.Questions.Count,
                    CreatorId = i.UserId,
                    CreatorFName = i.User.FirstName,
                    CreatorLName = i.User.LastName != null ? i.User.LastName : string.Empty,
                    CreatorAvatar = i.User.Image,
                    CreatorUsername = i.User.UserName,
                });
            });

            var interviewsVM = new AllInterviewsVM
            {
                Seniority = seniority,
                Interviews = interviewsDto
                .Select(i =>
                new InterviewVM
                {
                    InterviewId = i.InterviewId,
                    Seniority = i.SeniorityAsNumber.SeniorityNameParser(),
                    PositionTitle = i.PositionTitle.PositionTitleParser(),
                    Date = i.Date,
                    Likes = i.Likes,
                    Questions = i.Questions,
                    CreatorId = i.CreatorUsername != null ? i.CreatorId : string.Empty,
                    CreatorName = i.CreatorUsername != null ? i.CreatorFName.FullUserNameParser(i.CreatorLName) : GlobalConstants.UserDeleted,
                    CreatorAvatar = i.CreatorUsername != null ? i.CreatorAvatar != null ? i.CreatorAvatar : GlobalConstants.DefaultAvatar : null,
                    DisableUserLink = i.CreatorUsername != null ? string.Empty : GlobalConstants.DesableLink,
                }),
            };

            return interviewsVM;
        }

        public AllInterviewsVM AllByPage(int page, AllInterviewsVM interviewVM, IEnumerable<InterviewVM> interviews)
        {
            interviewVM.Interviews = interviewVM.SetPagination<InterviewVM>(interviews, page);

            return interviewVM;
        }

        public CreateInterviewVM CreateGetVM()
        {
            return new CreateInterviewVM
            {
                Questions = new List<CreateInterviewQuestionVM>
                {
                    new CreateInterviewQuestionVM
                    {
                        GivenAnswerCss = GlobalConstants.Hidden,
                        GivenAnswerBtnText = GlobalConstants.AddAnswer,
                    },
                },
            };
        }

        public async Task Create(CreateInterviewVM model, string userId, string fileDirectory, IFileService fileService)
        {
            var questions = new List<Question>();

            foreach (var q in model.Questions)
            {
                var fileName = await fileService.SaveFile(q.FormFile, fileDirectory);

                var rankValue = Math.Max(q.Interesting, Math.Max(q.Unexpected, q.Difficult));

                questions.Reverse();

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
                HeldOnInterviewLocation = locationType == LocationType.InOffice ? model.BasedPositionlocation : null,
                UserId = userId,
            };

            foreach (var q in questions)
            {
                interview.Questions.Add(q);
            }

            this.db.Interviews.Add(interview);
            this.db.SaveChanges();
        }

        public T Details<T>(string interviewId, string currentUserId, bool isAdmin)
        {
            var interviewDTO = this.interviewsRepository.All()
                .Where(i => i.Id == interviewId)
                .Select(i => new DetailsInterviewDTO
                {
                    InterviewId = i.Id,
                    UserId = i.UserId,
                    UserName = i.User.UserName,
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
                                UserName = c.User.UserName,
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
                        UserName = c.User.UserName,
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
                UserId = interviewDTO.UserName != null ? interviewDTO.UserId : null,
                UserFullName = interviewDTO.UserName != null ? interviewDTO.UserFName.FullUserNameParser(interviewDTO.UserLName) : GlobalConstants.UserDeleted,
                DisableUserLink = interviewDTO.UserName != null ? string.Empty : GlobalConstants.DesableLink,
                Seniority = Utils.ParseEnum<PositionSeniorityVM>(interviewDTO.Seniority),
                PositionTitle = interviewDTO.PositionTitle,
                PositionDescription = interviewDTO.PositionDescription == null ? GlobalConstants.NoDescription : interviewDTO.PositionDescription,
                CompanyNationality = interviewDTO.CompanyNationality,
                CompanySize = Utils.ParseEnum<EmployeesSizeVM>(interviewDTO.CompanySize),
                LocationType = Utils.ParseEnum<LocationTypeVM>(interviewDTO.LocationType),
                ShowLocation = interviewDTO.LocationType == LocationTypeVM.InOffice ? string.Empty : GlobalConstants.Hidden,
                InterviewLocation = interviewDTO.InterviewLocation,
                CreatedOn = interviewDTO.CreatedOn.DateTimeViewFormater(),
                ModifiedOn = interviewDTO.ModifiedOn?.DateTimeViewFormater(),
                HideAddCommentForm = Utils.HideAddComment(currentUserId),
                Likes = interviewDTO.Likes,
                AddLike = interviewDTO.IsLiked ? GlobalConstants.LikedCss : string.Empty,
                CanEdit = interviewDTO.UserId == currentUserId ? string.Empty : GlobalConstants.Hidden,
                CanDelete = (interviewDTO.UserId == currentUserId || isAdmin) ? string.Empty : GlobalConstants.Hidden,
                CanHardDelete = isAdmin ? string.Empty : GlobalConstants.Hidden,
                InterviewQns = interviewDTO.InterviewQns
                    .Select(q => new AllInterviewQuestionsVM
                    {
                        QuestionId = q.QuestionId,
                        Content = q.Content.SanitizeTextInput(),
                        Answer = q.Answer.SanitizeTextInput(),
                        HideAnswer = q.Answer == null,
                        CreatedOn = q.CreatedOn.DateTimeViewFormater(),
                        ModifiedOn = q.ModifiedOn?.DateTimeViewFormater(),
                        HideRanked = q.Ranked == 0,
                        Ranked = Utils.ParseEnum<QuestionRankTypeVM>(q.Ranked),
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

        public async Task<EditInterviewDTO> EditGet(string interviewId)
        {
            return await Task.Run(async () =>
            {
                var interviewDTO = this.interviewsRepository.All()
                .Where(i => i.Id == interviewId)
                .Select(i => new EditInterviewDTO
                {
                    InterviewId = i.Id,
                    Seniority = Enum.Parse<PositionSeniorityVM>(i.Seniority.ToString()),
                    PositionTitle = i.PositionTitle,
                    PositionDescription = i.PositionDescription,
                    TotalEmployees = Enum.Parse<EmployeesSizeVM>(i.Employees.ToString()),
                    LocationType = Enum.Parse<LocationTypeVM>(i.LocationType.ToString()),
                    InOfficeChecked = i.LocationType == LocationType.InOffice ? GlobalConstants.LocationTypeChecked : string.Empty,
                    RemoteChecked = i.LocationType == LocationType.Remote ? GlobalConstants.LocationTypeChecked : string.Empty,
                    ShowLocation = i.LocationType == LocationType.Remote ? GlobalConstants.Hidden : string.Empty,
                    BasedPositionlocation = i.HeldOnInterviewLocation,
                    CompanyNationality = i.CompanyNationality,
                    Questions = i.Questions
                        .Where(q => !q.IsDeleted)
                        .Select(q => new EditInterviewQuestionsDTO
                        {
                            QuestionId = q.Id,
                            Content = q.Content,
                            GivenAnswer = q.GivenAnswer,
                            Ranked = Enum.Parse<QuestionRankTypeVM>(q.RankType.ToString()),
                            File = q.UrlTask,
                        })
                        .ToList(),
                })
                .FirstOrDefault();

                interviewDTO.CompanyListNationalities = await this.importerHelperService.GetAll();

                return interviewDTO;
            });
        }

        public async Task Edit(EditInterviewDTO interviewDTO, string currentUserId, string fileDirectory, IFileService fileService)
        {
            var dbInterview = this.db.Interviews
                .Where(i => i.Id == interviewDTO.InterviewId)
                .Include(i => i.Questions)
                .ToList()
                .FirstOrDefault();

            if (dbInterview == null || dbInterview.UserId != currentUserId)
            {
                return;
            }

            LocationType locationType;

            if (!Enum.TryParse<LocationType>(interviewDTO.LocationType.ToString(), out locationType))
            {
                throw new ArgumentException($"Location type value: '{interviewDTO.LocationType}' is invalid!");
            }

            dbInterview.Seniority = (PositionSeniority)interviewDTO.Seniority;
            dbInterview.PositionTitle = interviewDTO.PositionTitle;
            dbInterview.PositionDescription = interviewDTO.PositionDescription;
            dbInterview.HeldOnDate = DateTime.UtcNow;
            dbInterview.CompanyNationality = interviewDTO.CompanyNationality;
            dbInterview.Employees = (EmployeesSize)interviewDTO.TotalEmployees;
            dbInterview.LocationType = locationType;
            dbInterview.HeldOnInterviewLocation = (locationType == LocationType.InOffice) ? interviewDTO.BasedPositionlocation : null;

            var allChangedQnsIds = interviewDTO.Questions.Where(q => q.QuestionId != null);

            foreach (var qdb in dbInterview.Questions)
            {
                var questionDTO = interviewDTO.Questions.FirstOrDefault(qu => qu.QuestionId != null && qu.QuestionId == qdb.Id);

                if (questionDTO != null)
                {
                    var rankValue = Math.Max(
                      questionDTO.Interesting,
                      Math.Max(
                      questionDTO.Unexpected,
                      questionDTO.Difficult));

                    var fileName = await fileService.SaveFile(questionDTO.FormFile, fileDirectory);

                    qdb.Content = questionDTO.Content;
                    qdb.GivenAnswer = questionDTO.GivenAnswer;
                    qdb.ModifiedOn = DateTime.UtcNow;
                    qdb.RankType = (QuestionRankType)rankValue;
                    qdb.UrlTask = fileName == null ? qdb.UrlTask : fileName;
                }
                else
                {
                    qdb.IsDeleted = true;
                }
            }

            this.db.Interviews.Update(dbInterview);
            await this.db.SaveChangesAsync();

            var allNewQns = interviewDTO.Questions.Where(q => q.QuestionId == null);

            if (allNewQns.Count() == 0)
            {
                return;
            }

            var newQuestions = new List<Question>();

            foreach (var q in allNewQns)
            {
                var fileName = await fileService.SaveFile(q.FormFile, fileDirectory);

                var rankValue = Math.Max(q.Interesting, Math.Max(q.Unexpected, q.Difficult));

                newQuestions.Add(new Question
                {
                    Content = q.Content,
                    GivenAnswer = q.GivenAnswer,
                    CreatedOn = DateTime.UtcNow,
                    RankType = (QuestionRankType)rankValue,
                    UrlTask = fileName,
                });
            }

            var dbInterviewForNewQns = this.db.Interviews
                .Where(i => i.Id == interviewDTO.InterviewId)
                .FirstOrDefault();

            foreach (var q in newQuestions)
            {
                dbInterviewForNewQns.Questions.Add(q);
            }

            this.db.Interviews.Update(dbInterviewForNewQns);
            await this.db.SaveChangesAsync();
        }

        public async Task Delete(string interviewId, string currentUserId, bool isAdmin)
        {
            var dbInterview = this.interviewsRepository.All()
                 .Include(i => i.Comments)
                 .Include(i => i.Likes)
                 .Include(i => i.Questions)
                 .ThenInclude(q => q.Comments)
                 .FirstOrDefault(i => i.Id == interviewId);

            if (dbInterview != null && (dbInterview.UserId == currentUserId || isAdmin))
            {
                foreach (var q in dbInterview.Questions)
                {
                    foreach (var c in q.Comments)
                    {
                        this.commentsEntityRepository.Delete(c);
                    }
                }

                await this.commentsEntityRepository.SaveChangesAsync();

                foreach (var q in dbInterview.Questions)
                {
                    this.questionsEntityRepository.Delete(q);
                }

                await this.questionsEntityRepository.SaveChangesAsync();

                foreach (var c in dbInterview.Comments)
                {
                    this.commentsEntityRepository.Delete(c);
                }

                await this.commentsEntityRepository.SaveChangesAsync();

                foreach (var l in dbInterview.Likes)
                {
                    this.likesEntityRepository.Delete(l);
                }

                await this.likesEntityRepository.SaveChangesAsync();

                this.interviewsRepository.Delete(dbInterview);

                await this.interviewsRepository.SaveChangesAsync();
            }
        }

        public async Task HardDelete(string interviewId, bool isAdmin)
        {
            var dbInterview = this.interviewsRepository.AllWithDeleted()
           .Include(i => i.Comments)
           .Include(i => i.Likes)
           .Include(i => i.Questions)
           .ThenInclude(q => q.Comments)
           .FirstOrDefault(i => i.Id == interviewId);

            if (dbInterview != null && isAdmin)
            {
                foreach (var q in dbInterview.Questions)
                {
                    foreach (var c in q.Comments)
                    {
                        this.commentsEntityRepository.HardDelete(c);
                    }
                }

                await this.commentsEntityRepository.SaveChangesAsync();

                foreach (var q in dbInterview.Questions)
                {
                    this.questionsEntityRepository.HardDelete(q);
                }

                await this.questionsEntityRepository.SaveChangesAsync();

                foreach (var c in dbInterview.Comments)
                {
                    this.commentsEntityRepository.HardDelete(c);
                }

                await this.commentsEntityRepository.SaveChangesAsync();

                foreach (var l in dbInterview.Likes)
                {
                    this.likesEntityRepository.HardDelete(l);
                }

                await this.likesEntityRepository.SaveChangesAsync();

                this.interviewsRepository.HardDelete(dbInterview);

                await this.interviewsRepository.SaveChangesAsync();
            }
        }

        public async Task AddComment(AddCommentDTO postComment, string userId)
        {
            var interview = await this.interviewsRepository.GetByIdWithDeletedAsync(postComment.Id);

            var comment = new Comment
            {
                InterviewId = interview.Id,
                Content = postComment.Content,
                CreatedOn = DateTime.UtcNow,
                UserId = userId,
            };

            interview.Comments.Add(comment);
            await this.interviewsRepository.SaveChangesAsync();
        }

        public T AllComments<T>(string interviewId, string currentUserId, bool isAdmin)
        {
            var commentsDTO = this.interviewsRepository.All()
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
            var liked = this.interviewsRepository.All()
                 .Where(i => i.Id == interviewId)
                 .SelectMany(i => i.Likes)
                 .FirstOrDefault(l => l.UserId == userId);

            bool isLiked = false;

            if (liked == null)
            {
                var interview = this.interviewsRepository.All()
              .FirstOrDefault(i => i.Id == interviewId);

                var like = new Like
                {
                    InterviewId = interviewId,
                    UserId = userId,
                    CreatedOn = DateTime.UtcNow,
                    IsLiked = true,
                };

                interview.Likes.Add(like);
                await this.interviewsRepository.SaveChangesAsync();

                isLiked = true;
            }
            else if (!liked.IsLiked)
            {
                liked.IsLiked = true;
                await this.interviewsRepository.SaveChangesAsync();

                isLiked = true;
            }
            else if (liked.IsLiked)
            {
                liked.IsLiked = false;
                await this.interviewsRepository.SaveChangesAsync();

                isLiked = false;
            }

            var count = this.interviewsRepository.All()
                .Where(i => i.Id == interviewId)
                .SelectMany(i => i.Likes)
                .Where(l => l.IsLiked).Count();

            return new LikeVM
            {
                Count = count,
                LikedCss = isLiked ? GlobalConstants.LikedCss : string.Empty,
            };
        }
    }
}
