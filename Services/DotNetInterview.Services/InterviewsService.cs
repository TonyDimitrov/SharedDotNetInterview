namespace DotNetInterview.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using DotNetInterview.Common;
    using DotNetInterview.Data;
    using DotNetInterview.Data.Common.Repositories;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Data.Models.Enums;
    using DotNetInterview.Web.ViewModels.Enums;
    using DotNetInterview.Web.ViewModels.Interviews;
    using DotNetInterview.Web.ViewModels.Interviews.DTO;
    using DotNetInterview.Web.ViewModels.Questions;
    using DotNetInterview.Web.ViewModels.Questions.DTO;
    using Microsoft.AspNetCore.Http;

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
                    Likes = i.Likes,
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
                   Likes = i.Likes,
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
                    Questions = i.Questions,
                    CreatorId = i.CreatorId,
                    CreatorName = this.FullUserNameParser(i.CreatorFName, i.CreatorLName),
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
            //var nationalityes = this.db.Nationalities
            //    .Select(n => n.CompanyNationality)
            //    .ToList()
            //    .OrderBy(n => n);

            //var getDataVM = new GetCreateInterviewsVM
            //{
            //    Nationality = nationalityes,
            //};

            return new CreateInterviewVM
            {
                Questions = new List<CreateInterviewQuestionVM> { new CreateInterviewQuestionVM() },
                Select = new GetCreateInterviewsVM(),
            };
        }

        public T Details<T>(string interviewId)
        {
            var interviewDTO = this.categoriesRepository.All()
                .Where(i => i.Id == interviewId && !i.IsDeleted)
                .Select(i => new DetailsInterviewDTO
                {
                    InterviewId = i.Id,
                    Seniority = Enum.Parse<PositionSeniorityVM>(i.Seniority.ToString()),
                    PositionTitle = i.PositionTitle,
                    PositionDescription = i.PositionDescription,
                    CompanyNationality = i.CompanyNationality,
                    CompanySize = Enum.Parse<EmployeesSizeVM>(i.Employees.ToString()),
                    LocationType = Enum.Parse<LocationTypeVM>(i.LocationType.ToString()),
                    InterviewLocation = i.HeldOnInterviewLocation,
                    CreatedOn = i.CreatedOn,
                    ModifiedOn = i.ModifiedOn,
                    Likes = i.Likes,
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
                            QnsComments = q.Comments
                            .Where(q => !q.IsDeleted)
                            .Select(c => new AllQuestionCommentsDTO
                            {
                                Content = c.Content,
                                CreatedOn = c.CreatedOn,
                                ModifiedOn = c.ModifiedOn,
                            })
                            .ToList(),
                        })
                        .ToList(),
                    InterviewComments = i.Comments
                    .Select(c => new AllInterviewCommentsDTO
                    {
                        Content = c.Content,
                        CreatedOn = c.CreatedOn,
                        ModifiedOn = c.ModifiedOn,
                    })
                    .ToList(),
                })
                .FirstOrDefault();

            var interviewVM = new DetailsInterviewVM
            {
                InterviewId = interviewDTO.InterviewId,
                Seniority = Helper.ParseEnum<PositionSeniorityVM>(interviewDTO.Seniority),
                PositionTitle = interviewDTO.PositionTitle,
                PositionDescription = interviewDTO.PositionDescription == null ? "No description" : interviewDTO.PositionDescription,
                CompanyNationality = interviewDTO.CompanyNationality,
                CompanySize = Helper.ParseEnum<EmployeesSizeVM>(interviewDTO.CompanySize),
                LocationType = Helper.ParseEnum<LocationTypeVM>(interviewDTO.LocationType),
                InterviewLocation = interviewDTO.InterviewLocation,
                CreatedOn = interviewDTO.CreatedOn.ToString(GlobalConstants.FormatDate),
                ModifiedOn = interviewDTO.ModifiedOn?.ToString(GlobalConstants.FormatDate),
                Likes = interviewDTO.Likes,
                InterviewQns = interviewDTO.InterviewQns
                    .Select(q => new AllInterviewQuestionsVM
                    {
                        QuestionId = q.QuestionId,
                        Content = q.Content,
                        Answer = q.Answer,
                        CreatedOn = q.CreatedOn.ToString(GlobalConstants.FormatDate),
                        ModifiedOn = q.ModifiedOn?.ToString(GlobalConstants.FormatDate),
                        Ranked = (int)q.Ranked,
                        File = q.File,
                        QnsComments = q.QnsComments
                        .Select(c => new AllQuestionCommentsVM
                        {
                            QuestionId = c.QuestionId,
                            Content = c.Content,
                            CreatedOn = c.CreatedOn.ToString(GlobalConstants.FormatDate),
                            ModifiedOn = c.ModifiedOn?.ToString(GlobalConstants.FormatDate),
                        }),
                    }),
                InterviewComments = interviewDTO.InterviewComments
                    .Select(c => new AllInterviewCommentsVM
                    {
                        Content = c.Content,
                        CreatedOn = c.CreatedOn.ToString(GlobalConstants.FormatDate),
                        ModifiedOn = c.ModifiedOn?.ToString(GlobalConstants.FormatDate),
                    })
                    .ToList(),
            };

            return (T)(object)interviewVM;
        }

        public async Task AddComment(AddInterviewComment interviewComment, string userId)
        {
            var interview = await this.categoriesRepository.GetByIdWithDeletedAsync(interviewComment.InterviewId);

            var comment = new Comment
            {
                InterviewId = interview.Id,
                Content = interviewComment.Content,
                CreatedOn = DateTime.UtcNow,
                UserId = userId,
            };

            interview.Comments.Add(comment);
            await this.categoriesRepository.SaveChangesAsync();
        }

        public T AllInterviewComments<T>(string interviewId)
        {
            var commentsDTO = this.categoriesRepository.All()
               .Where(i => i.Id == interviewId)
               .FirstOrDefault()
               .Comments
               .Where(c => !c.IsDeleted)
               .Select(c => new AllInterviewCommentsDTO
               {
                   InterviewId = interviewId,
                   Content = c.Content,
                   CreatedOn = c.CreatedOn,
                   ModifiedOn = c.ModifiedOn,
                   UserId = c.UserId,
                   UserFName = c.User.FirstName,
                   UserLName = c.User.LastName,
               })
               .ToList();

            var commentsVM = this.categoriesRepository.All()
              .Where(i => i.Id == interviewId)
              .FirstOrDefault()
              .Comments
              .Where(c => !c.IsDeleted)
              .Select(c => new AllInterviewCommentsVM
              {
                  InterviewId = interviewId,
                  Content = c.Content,
                  CreatedOn = c.CreatedOn.ToString(GlobalConstants.FormatDate),
                  ModifiedOn = c.ModifiedOn?.ToString(GlobalConstants.FormatDate),
                  UserId = c.UserId,
                  UserFullName = this.FullUserNameParser(c.User.FirstName, c.User.LastName),
              })
              .ToList();

            return (T)(object)commentsVM;
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
                6 => "Other",
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

        private string FullUserNameParser(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
            {
                if (firstName.Length <= 20)
                {
                    return firstName;
                }
                else
                {
                    return firstName.Substring(0, 17) + "...";
                }
            }
            else
            {
                var fullName = $"{firstName} {lastName.Substring(0, 1).ToUpper()}.";

                if (fullName.Length <= 20)
                {
                    return fullName;
                }
                else
                {
                    return fullName.Substring(0, 17) + "...";
                }
            }
        }
    }
}
