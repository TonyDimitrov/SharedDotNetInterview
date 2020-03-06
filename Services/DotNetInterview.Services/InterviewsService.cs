namespace DotNetInterview.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using DotNetInterview.Data;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Data.Models.Enums;
    using DotNetInterview.Web.ViewModels.Interviews;
    using DotNetInterview.Web.ViewModels.Interviews.DTO;
    using Microsoft.AspNetCore.Http;

    public class InterviewsService : IInterviewsService
    {
        private readonly ApplicationDbContext db;

        public InterviewsService(ApplicationDbContext db)
        {
            this.db = db;
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
                     Date = i.HeldOnDate.ToString("dd MMM yyyy hh:mm"),
                     Likes = i.Likes,
                     Questions = i.Questions.Count,
                     CreatorId = i.UserId,
                     CreatorFName = i.User.FirstName,
                     CreatorLName = i.User.LastName != null ? i.User.LastName : string.Empty,
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
                })
                .ToList(),
            };

            return (T)(object)interviewsVM;
        }

        public async Task Create(CreateInterviewVM model, string userId, string filePath)
        {
            var questions = new List<Question>();

            foreach (var q in model.Questions)
            {
                string fileName = null;

                if (q?.FormFile != null && q.FormFile.FileName != null)
                {
                    fileName = this.UniqueFileNameGenerator(q.FormFile);

                    using (var stream = new FileStream(filePath + fileName, FileMode.CreateNew))
                    {
                        await q.FormFile.CopyToAsync(stream);
                    }
                }

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
            var nationalityes = this.db.Nationalities
                .Select(n => n.CompanyNationality)
                .ToList()
                .OrderBy(n => n);

            var getVM = new GetCreateInterviewsVM
            {
                Nationality = nationalityes,
            };

            return new CreateInterviewVM
            {
                Questions = new List<CreateInterviewQuestionVM> { new CreateInterviewQuestionVM() },
                Select = getVM,
            };
        }

        private string UniqueFileNameGenerator(IFormFile file)
        {
            var fileExtension = file.FileName
                .Split(".")
                .LastOrDefault();

            var uniqueFileName = Guid.NewGuid().ToString() + "." + fileExtension;

            return uniqueFileName;
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
                var fullName = $"{firstName} {lastName}";

                if ((firstName.Length + lastName.Length) <= 20)
                {
                    return fullName;
                }
                else
                {
                    return fullName.Substring(0, 17) + "...";
                }
            }
        }

        private string ImageUserUrlParser(IEnumerable<Image> images, string imagesPath)
        {
            var imageTitle = images.Where(i => i.IsDeleted == false).Select(i => i.ImageUrl).FirstOrDefault();

            if (imageTitle != null)
            {
                return imagesPath + imageTitle;
            }

            return null;
        }

        private byte[] GetByteArrayFromImage(IFormFile file)
        {
            using (var target = new MemoryStream())
            {
                file.CopyTo(target);
                return target.ToArray();
            }
        }
    }
}
