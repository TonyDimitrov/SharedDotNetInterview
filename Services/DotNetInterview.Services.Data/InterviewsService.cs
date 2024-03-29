﻿namespace DotNetInterview.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DotNetInterview.Common;
    using DotNetInterview.Data;
    using DotNetInterview.Data.Common.Repositories;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Data.Models.Enums;
    using DotNetInterview.Services.Data.Extensions;
    using DotNetInterview.Services.Data.Helpers;
    using DotNetInterview.Web.Infrastructure.Extensions;
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
        private readonly IDeletableEntityRepository<Question> questionsRepository;
        private readonly IDeletableEntityRepository<Comment> commentsRepository;
        private readonly IDeletableEntityRepository<Like> likesRepository;
        private readonly INationalitiesService nationalitiesService;

        public InterviewsService(
            ApplicationDbContext db,
            IDeletableEntityRepository<Interview> categoriesRepository,
            IDeletableEntityRepository<Question> questionsRepository,
            IDeletableEntityRepository<Comment> commentsRepository,
            IDeletableEntityRepository<Like> likesRepository,
            INationalitiesService nationalitiesService)
        {
            this.db = db;
            this.interviewsRepository = categoriesRepository;
            this.questionsRepository = questionsRepository;
            this.commentsRepository = commentsRepository;
            this.likesRepository = likesRepository;
            this.nationalitiesService = nationalitiesService;
        }

        public async Task<AllInterviewsVM> All(int seniority)
        {
            var selectAllSeniorities = seniority == 0;

            var interviewsDto = await Task.Run(() =>
            {
                return this.interviewsRepository.All()
                .Where(i => (int)(object)i.Seniority == seniority || selectAllSeniorities)
                .OrderByDescending(i => i.CreatedOn)
                .Select(i => new AllInterviewDTO
                {
                    InterviewId = i.Id,
                    PositionTitle = i.PositionTitle,
                    PositionSeniority = (PositionSeniorityImgVM)(int)i.Seniority,
                    Date = i.CreatedOn,
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

            var interviewsVM = new AllInterviewsVM(seniority)
            {
                Interviews = interviewsDto
                .Select(i =>
                new InterviewVM
                {
                    InterviewId = i.InterviewId,
                    Seniority = i.PositionSeniority.DisplayName(),
                    SeniorityTooltip = i.PositionSeniority.DisplayTooltipClass(),
                    PositionTitle = i.PositionTitle.PositionTitleParser(),
                    Date = i.Date.DateTimeViewFormater(),
                    Likes = i.Likes,
                    Questions = i.Questions,
                    CreatorId = i.CreatorUsername != null ? i.CreatorId : string.Empty,
                    CreatorName = i.CreatorUsername != null ? i.CreatorFName.FullUserNameParser(i.CreatorLName) : GlobalConstants.UserDeleted,
                    CreatorAvatar = i.CreatorUsername != null ? i.CreatorAvatar != null ? i.CreatorAvatar : GlobalConstants.DefaultAvatar : null,
                    DisableUserLink = i.CreatorUsername != null ? string.Empty : GlobalConstants.DisableLink,
                }),
            };

            interviewsVM.Nationalities = await this.nationalitiesService.GetAll();

            return interviewsVM;
        }

        public AllInterviewsVM AllByPage(int page, AllInterviewsVM interviewVM, IEnumerable<InterviewVM> interviews)
        {
            interviewVM.Interviews = interviewVM.SetPagination<InterviewVM>(interviews, page);

            return interviewVM;
        }

        public async Task<AllInterviewsVM> AllByFilter(AllAjaxInterviewDTO interviewDTO)
        {

            var interviewsDto = await Task.Run(() =>
            {
                var interviewsDb = this.interviewsRepository.All();

                if (interviewDTO.Seniority != 0)
                {
                    interviewsDb = interviewsDb.Where(i => i.Seniority == (PositionSeniority)interviewDTO.Seniority);
                }

                if (interviewDTO.NationalityId.HasValue && interviewDTO.NationalityId != 0)
                {
                    interviewsDb = interviewsDb.Where(i => i.NationalityId == interviewDTO.NationalityId);
                }

                if (interviewDTO.From.HasValue)
                {
                    interviewsDb = interviewsDb.Where(i => i.HeldOnDate >= interviewDTO.From.Value);
                }

                if (interviewDTO.To.HasValue)
                {
                    interviewsDb = interviewsDb.Where(i => i.HeldOnDate <= interviewDTO.To.Value);
                }

                var interviewsDto = interviewsDb.OrderByDescending(i => i.CreatedOn)
                .Select(i => new AllInterviewDTO
                {
                    InterviewId = i.Id,
                    PositionTitle = i.PositionTitle,
                    PositionSeniority = (PositionSeniorityImgVM)(int)i.Seniority,
                    Date = i.CreatedOn,
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

                return interviewsDto;
            });

            var interviewsVM = new AllInterviewsVM(interviewDTO.Seniority)
            {
                NationalityId = interviewDTO.NationalityId,
                From = interviewDTO.From,
                To = interviewDTO.To,
                Interviews = interviewsDto
                .Select(i =>
                new InterviewVM
                {
                    InterviewId = i.InterviewId,
                    Seniority = i.PositionSeniority.DisplayName(),
                    SeniorityTooltip = i.PositionSeniority.DisplayTooltipClass(),
                    PositionTitle = i.PositionTitle.PositionTitleParser(),
                    Date = i.Date.DateTimeViewFormater(),
                    Likes = i.Likes,
                    Questions = i.Questions,
                    CreatorId = i.CreatorUsername != null ? i.CreatorId : string.Empty,
                    CreatorName = i.CreatorUsername != null ? i.CreatorFName.FullUserNameParser(i.CreatorLName) : GlobalConstants.UserDeleted,
                    CreatorAvatar = i.CreatorUsername != null ? i.CreatorAvatar != null ? i.CreatorAvatar : GlobalConstants.DefaultAvatar : null,
                    DisableUserLink = i.CreatorUsername != null ? string.Empty : GlobalConstants.DisableLink,
                }),
            };

            interviewsVM.Nationalities = await this.nationalitiesService.GetAllWithSelected(interviewDTO.NationalityId);

            return interviewsVM;
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

                var question = new Question
                {
                    Content = q.Content,
                    GivenAnswer = q.GivenAnswer,
                    CreatedOn = DateTime.UtcNow,
                    RankType = (QuestionRankType)rankValue,
                    UrlTask = fileName,
                };

                await this.questionsRepository.AddAsync(question);
                await this.questionsRepository.SaveChangesAsync();

                questions.Add(question);
            }

            LocationType locationType;

            if (!Enum.TryParse<LocationType>(model.LocationType, out locationType))
            {
                throw new ArgumentException($"Location type value: '{model.LocationType}' is invalid!");
            }

            if (!int.TryParse(model.CompanyNationalityId, out var nationalityId))
            {
                throw new ArgumentException($"Company nationality Id : '{model.CompanyNationalityId}' is invalid!");
            }

            var nationality = await this.nationalitiesService.GetById(nationalityId);

            var interview = new Interview
            {
                Seniority = (PositionSeniority)model.Seniority,
                PositionTitle = model.PositionTitle,
                PositionDescription = model.PositionDescription,
                HeldOnDate = model.HodlOnDate,
                CreatedOn = DateTime.UtcNow,
                Nationality = nationality,
                CompanyNationality = nationality?.CompanyNationality,
                Employees = (EmployeesSize)model.Employees,
                LocationType = locationType,
                BasedPositionLocation = locationType == LocationType.InOffice ? model.BasedPositionLocation : null,
                UserId = userId,
            };

            foreach (var q in questions)
            {
                interview.Questions.Add(q);
            }

            await this.interviewsRepository.AddAsync(interview);
            await this.interviewsRepository.SaveChangesAsync();
        }

        public DetailsInterviewVM Details(string interviewId, string currentUserId, bool isAdmin)
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
                    Seniority = i.Seniority.ToString(),
                    SeniorityImg = (PositionSeniorityImgVM)(int)i.Seniority,
                    PositionTitle = i.PositionTitle,
                    PositionDescription = i.PositionDescription,
                    CompanyNationality = i.Nationality.CompanyNationality,
                    CompanySize = i.Employees.ToString(),
                    LocationType = i.LocationType.ToString(),
                    InterviewLocation = i.BasedPositionLocation,
                    HeldOn = i.HeldOnDate,
                    CreatedOn = i.CreatedOn,
                    ModifiedOn = i.ModifiedOn,
                    Likes = i.Likes
                    .Where(l => l.IsLiked)
                    .Count(),
                    IsLiked = i.Likes
                    .FirstOrDefault(l => l.UserId == currentUserId && l.IsLiked) != null ? true : false,
                    InterviewQns = i.Questions
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
                            .Select(c => new AllCommentsDTO
                            {
                                CommentId = c.Id,
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
                        .OrderBy(q => q.CreatedOn)
                        .ToList(),
                    InterviewComments = i.Comments
                    .Select(c => new AllCommentsDTO
                    {
                        CommentId = c.Id,
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

            if (interviewDTO == null)
            {
                return null;
            }

            var interviewVM = new DetailsInterviewVM
            {
                InterviewId = interviewDTO.InterviewId,
                UserId = interviewDTO.UserName != null ? interviewDTO.UserId : null,
                UserFullName = interviewDTO.UserName != null ? interviewDTO.UserFName.FullUserNameParser(interviewDTO.UserLName) : GlobalConstants.UserDeleted,
                DisableUserLink = interviewDTO.UserName != null ? string.Empty : GlobalConstants.DisableLink,
                Seniority = interviewDTO.Seniority,
                SeniorityImg = interviewDTO.SeniorityImg.DisplayName(),
                PositionTitle = interviewDTO.PositionTitle,
                PositionDescription = interviewDTO.PositionDescription == null ? GlobalConstants.NoDescription : interviewDTO.PositionDescription,
                CompanyNationality = interviewDTO.CompanyNationality,
                CompanySize = interviewDTO.CompanySize,
                LocationType = interviewDTO.LocationType,
                ShowLocation = interviewDTO.LocationType == GlobalConstants.LocationTypeInOffice ? string.Empty : GlobalConstants.Hidden,
                BasedPositionLocation = interviewDTO.InterviewLocation,
                HeldOn = interviewDTO.HeldOn.DateTimeViewFormater(),
                CreatedOn = interviewDTO.CreatedOn.DateTimeViewFormater(),
                ModifiedOn = interviewDTO.ModifiedOn?.DateTimeViewFormater(),
                HideAddCommentForm = Utils.HideAddComment(currentUserId),
                Likes = interviewDTO.Likes,
                AddLike = interviewDTO.IsLiked ? GlobalConstants.LikedCss : string.Empty,
                CanEdit = interviewDTO.UserId == currentUserId ? string.Empty : GlobalConstants.Hidden,
                CanDelete = (interviewDTO.UserId == currentUserId || isAdmin) ? string.Empty : GlobalConstants.Hidden,
                CanHardDelete = isAdmin ? string.Empty : GlobalConstants.Hidden,
                QuestionRanks = Utils.AllQuestionRanks(),
                InterviewQns = interviewDTO.InterviewQns
                    .Select(q => new AllInterviewQuestionsVM
                    {
                        QuestionId = q.QuestionId,
                        Content = q.Content.SanitizeTextInput(),
                        Answer = q.Answer.SanitizeTextInput(),
                        HideAnswer = q.Answer == null,
                        CreatedOn = q.CreatedOn.DateTimeViewFormater(),
                        ModifiedOn = q.ModifiedOn?.DateTimeViewFormater(),
                        HideRanked = q.Ranked == GlobalConstants.None,
                        Ranked = q.Ranked,
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

            return interviewVM;
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
                    Employees = Enum.Parse<EmployeesSizeVM>(i.Employees.ToString()),
                    LocationType = Enum.Parse<LocationTypeVM>(i.LocationType.ToString()),
                    InOfficeChecked = i.LocationType == LocationType.InOffice ? GlobalConstants.LocationTypeChecked : string.Empty,
                    RemoteChecked = i.LocationType == LocationType.Remote ? GlobalConstants.LocationTypeChecked : string.Empty,
                    HodlOnDate = i.HeldOnDate,
                    ShowLocation = i.LocationType == LocationType.Remote ? GlobalConstants.Hidden : string.Empty,
                    BasedPositionLocation = i.BasedPositionLocation,
                    CompanyNationalityId = i.Nationality != null ? i.Nationality.Id : null,
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

                if (interviewDTO == null)
                {
                    return null;
                }

                interviewDTO.CompanyListNationalities = await this.nationalitiesService.GetAllWithSelected(interviewDTO.CompanyNationalityId);

                return interviewDTO;
            });
        }

        public async Task Edit(EditInterviewDTO interviewDTO, string currentUserId, string fileDirectory, IFileService fileService)
        {
            var dbInterview = this.interviewsRepository.All()
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

            if (!interviewDTO.CompanyNationalityId.HasValue || interviewDTO.CompanyNationalityId <= 0)
            {
                throw new ArgumentException($"Company nationality Id : '{interviewDTO.CompanyNationalityId}' is invalid!");
            }

            var nationality = await this.nationalitiesService.GetById(interviewDTO.CompanyNationalityId.Value);

            dbInterview.Seniority = (PositionSeniority)interviewDTO.Seniority;
            dbInterview.PositionTitle = interviewDTO.PositionTitle;
            dbInterview.PositionDescription = interviewDTO.PositionDescription;
            dbInterview.ModifiedOn = DateTime.UtcNow;
            dbInterview.HeldOnDate = interviewDTO.HodlOnDate;
            dbInterview.CompanyNationality = nationality?.CompanyNationality;
            dbInterview.NationalityId = nationality.Id;
            dbInterview.Nationality = nationality;
            dbInterview.Employees = (EmployeesSize)interviewDTO.Employees;
            dbInterview.LocationType = locationType;
            dbInterview.BasedPositionLocation = (locationType == LocationType.InOffice) ? interviewDTO.BasedPositionLocation : null;

            var allChangedQnsIds = interviewDTO.Questions.Where(q => q.QuestionId != null);

            var dbListInterviews = dbInterview.Questions.ToArray();

            for (var qdb = dbListInterviews.Length - 1; qdb >= 0; qdb--)
            {
                var questionDTO = interviewDTO.Questions.FirstOrDefault(qu => qu.QuestionId != null && qu.QuestionId == dbListInterviews[qdb].Id);

                if (questionDTO != null)
                {
                    var rankValue = Math.Max(
                      questionDTO.Interesting,
                      Math.Max(
                      questionDTO.Unexpected,
                      questionDTO.Difficult));

                    var fileName = await fileService.SaveFile(questionDTO.FormFile, fileDirectory);

                    dbListInterviews[qdb].Content = questionDTO.Content;
                    dbListInterviews[qdb].GivenAnswer = questionDTO.GivenAnswer;
                    dbListInterviews[qdb].ModifiedOn = DateTime.UtcNow;
                    dbListInterviews[qdb].RankType = (QuestionRankType)rankValue;
                    dbListInterviews[qdb].UrlTask = fileName == null ? dbListInterviews[qdb].UrlTask : fileName;
                }
                else
                {
                    dbInterview.Questions.Remove(dbListInterviews[qdb]);
                }
            }

            this.interviewsRepository.Update(dbInterview);
            await this.interviewsRepository.SaveChangesAsync();

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

            var dbInterviewForNewQns = this.interviewsRepository.All()
                .Where(i => i.Id == interviewDTO.InterviewId)
                .FirstOrDefault();

            foreach (var q in newQuestions)
            {
                dbInterviewForNewQns.Questions.Add(q);
            }

            this.interviewsRepository.Update(dbInterviewForNewQns);
            await this.interviewsRepository.SaveChangesAsync();
        }

        public async Task Delete(string interviewId, string currentUserId, bool isAdmin)
        {
            var dbInterview = this.interviewsRepository.All()
                 .Include(i => i.Comments)
                 .Include(i => i.Likes)
                 .Include(i => i.Questions)
                 .ThenInclude(q => q.Comments)
                 .FirstOrDefault(i => i.Id == interviewId);

            if (dbInterview == null)
            {
                return;
            }

            if (dbInterview != null && (dbInterview.UserId == currentUserId || isAdmin))
            {
                foreach (var q in dbInterview.Questions)
                {
                    foreach (var c in q.Comments)
                    {
                        this.commentsRepository.Delete(c);
                    }
                }

                await this.commentsRepository.SaveChangesAsync();

                foreach (var q in dbInterview.Questions)
                {
                    this.questionsRepository.Delete(q);
                }

                await this.questionsRepository.SaveChangesAsync();

                foreach (var c in dbInterview.Comments)
                {
                    this.commentsRepository.Delete(c);
                }

                await this.commentsRepository.SaveChangesAsync();

                foreach (var l in dbInterview.Likes)
                {
                    this.likesRepository.Delete(l);
                }

                await this.likesRepository.SaveChangesAsync();

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

            if (dbInterview == null)
            {
                return;
            }

            if (dbInterview != null && isAdmin)
            {
                foreach (var q in dbInterview.Questions)
                {
                    foreach (var c in q.Comments)
                    {
                        this.commentsRepository.HardDelete(c);
                    }
                }

                await this.commentsRepository.SaveChangesAsync();

                foreach (var q in dbInterview.Questions)
                {
                    this.questionsRepository.HardDelete(q);
                }

                await this.questionsRepository.SaveChangesAsync();

                foreach (var c in dbInterview.Comments)
                {
                    this.commentsRepository.HardDelete(c);
                }

                await this.commentsRepository.SaveChangesAsync();

                foreach (var l in dbInterview.Likes)
                {
                    this.likesRepository.HardDelete(l);
                }

                await this.likesRepository.SaveChangesAsync();

                this.interviewsRepository.HardDelete(dbInterview);

                await this.interviewsRepository.SaveChangesAsync();
            }
        }

        public async Task AddComment(AddCommentDTO postComment, string userId)
        {
            var interview = this.interviewsRepository.All()
                .FirstOrDefault(i => i.Id == postComment.Id);

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
               .Select(i => i.Comments
               .Select(c => new AllCommentsDTO
               {
                   CommentId = c.Id,
                   ParentId = interviewId,
                   Content = c.Content,
                   CreatedOn = c.CreatedOn,
                   ModifiedOn = c.ModifiedOn,
                   UserId = c.UserId,
                   UserFName = c.User.FirstName,
                   UserLName = c.User.LastName,
               }).OrderBy(c => c.CreatedOn)
               .ToList())
               .FirstOrDefault();

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
