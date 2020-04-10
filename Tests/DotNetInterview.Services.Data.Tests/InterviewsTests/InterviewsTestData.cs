namespace DotNetInterview.Services.Data.Tests.InterviewsTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using DotNetInterview.Data.Models;
    using DotNetInterview.Data.Models.Enums;
    using DotNetInterview.Web.ViewModels.Enums;
    using DotNetInterview.Web.ViewModels.Interviews;

    public class InterviewsTestData
    {
        public static IQueryable<Interview> GetInterviewsTestData()
        {
            var data = new List<Interview>
            {
                new Interview
                {
                    Id = "1",
                    Seniority = PositionSeniority.JuniorDeveloper,
                    PositionTitle = "Junior with some experience",
                    PositionDescription = "Junior with some experience on .net core",
                    LocationType = LocationType.InOffice,
                    BasedPositionLocation = "Sofia",
                    CreatedOn = new DateTime(2016, 05, 15, 10, 10, 10, DateTimeKind.Utc),
                    ModifiedOn = new DateTime(2016, 05, 15, 10, 10, 10, DateTimeKind.Utc),
                    CompanyNationality = "Bulgarian",
                    Employees = EmployeesSize.Between100And1000,
                    IsDeleted = false,
                    UserId = "1",
                    User = new ApplicationUser
                    {
                        Id = "1",
                        Email = "toni@toni.com",
                        UserName = "toni@toni.com",
                        FirstName = "Toni",
                        LastName = "Dimitrov",
                        IsDeleted = false,
                        Image = "avatar",
                    },
                    Questions = new List<Question>
                    {
                        new Question
                        {
                            Id = "1",
                            Content = "Waht is Encapsulation?",
                            GivenAnswer = "Data hiding",
                            RankType = QuestionRankType.MostUnexpected,
                            IsDeleted = false,
                            CreatedOn = new DateTime(2015, 05, 15, 10, 10, 10, DateTimeKind.Utc),
                            ModifiedOn = new DateTime(2015, 05, 15, 10, 10, 10, DateTimeKind.Utc),
                            InterviewId = "1",
                            UrlTask = "file.txt",
                        },
                        new Question
                        {
                            Id = "2",
                            Content = "Waht is Polymorphism?",
                            GivenAnswer = "Overrading methods",
                            RankType = QuestionRankType.MostInteresting,
                            IsDeleted = false,
                            CreatedOn = new DateTime(2015, 05, 15, 10, 10, 10, DateTimeKind.Utc),
                            ModifiedOn = new DateTime(2015, 05, 15, 10, 10, 10, DateTimeKind.Utc),
                            InterviewId = "1",
                            Comments = new List<Comment>
                            {
                                new Comment
                                {
                                Id = "1",
                                Content = "Not a difficult questions",
                                CreatedOn = new DateTime(2015, 05, 15, 10, 10, 10, DateTimeKind.Utc),
                                ModifiedOn = new DateTime(2015, 05, 15, 10, 10, 10, DateTimeKind.Utc),
                                IsDeleted = false,
                                QuestionId = "2",
                                InterviewId = "1",
                                UserId = "1",
                                User = new ApplicationUser
                                {
                                    Id = "1",
                                    Email = "toni@toni.com",
                                    UserName = "toni@toni.com",
                                    FirstName = "Toni",
                                    LastName = "Dimitrov",
                                    IsDeleted = false,
                                    Image = "avatar",
                                },
                                },
                                new Comment
                                {
                                Id = "1",
                                Content = "Regular questions",
                                CreatedOn = new DateTime(2015, 05, 15, 10, 10, 10, DateTimeKind.Utc),
                                ModifiedOn = new DateTime(2015, 05, 15, 10, 10, 10, DateTimeKind.Utc),
                                IsDeleted = true,
                                DeletedOn = new DateTime(2015, 05, 15, 10, 10, 10, DateTimeKind.Utc),
                                QuestionId = "2",
                                InterviewId = "1",
                                UserId = "1",
                                User = new ApplicationUser
                                {
                                    Id = "1",
                                    Email = "toni@toni.com",
                                    UserName = "toni@toni.com",
                                    FirstName = "Toni",
                                    LastName = "Dimitrov",
                                    IsDeleted = false,
                                    Image = "avatar",
                                },
                                },
                            },
                        },
                    },
                    Comments = new List<Comment>
                    {
                        new Comment
                        {
                            Id = "1",
                            Content = "Very short interview",
                            CreatedOn = new DateTime(2015, 05, 15, 10, 10, 10, DateTimeKind.Utc),
                            ModifiedOn = new DateTime(2015, 05, 15, 10, 10, 10, DateTimeKind.Utc),
                            IsDeleted = false,
                            InterviewId = "1",
                            UserId = "1",
                            User = new ApplicationUser
                            {
                                Id = "1",
                                Email = "toni@toni.com",
                                UserName = "toni@toni.com",
                                FirstName = "Toni",
                                LastName = "Dimitrov",
                                IsDeleted = false,
                                Image = "avatar",
                            },
                        },
                        new Comment
                        {
                            Id = "2",
                            Content = "not enough",
                            CreatedOn = new DateTime(2015, 05, 15, 10, 10, 10, DateTimeKind.Utc),
                            ModifiedOn = new DateTime(2015, 05, 15, 10, 10, 10, DateTimeKind.Utc),
                            IsDeleted = true,
                            InterviewId = "1",
                            UserId = "1",
                            User = new ApplicationUser
                            {
                                Id = "1",
                                Email = "toni@toni.com",
                                UserName = "toni@toni.com",
                                FirstName = "Toni",
                                LastName = "Dimitrov",
                                IsDeleted = false,
                                Image = "avatar",
                            },
                        },
                    },
                    Likes = new List<Like>
                    {
                         new Like
                         {
                              Id = "1",
                              IsLiked = true,
                              CreatedOn = new DateTime(2015, 05, 15, 10, 10, 10, DateTimeKind.Utc),
                              ModifiedOn = new DateTime(2015, 05, 15, 10, 10, 10, DateTimeKind.Utc),
                              IsDeleted = false,
                              InterviewId = "1",
                              UserId = "1",
                              User = new ApplicationUser
                              {
                                Id = "1",
                                Email = "toni@toni.com",
                                UserName = "toni@toni.com",
                                FirstName = "Toni",
                                LastName = "Dimitrov",
                                IsDeleted = false,
                                Image = "avatar",
                              },
                         },
                         new Like
                         {
                              Id = "2",
                              IsLiked = true,
                              CreatedOn = new DateTime(2015, 05, 15, 10, 10, 10, DateTimeKind.Utc),
                              ModifiedOn = new DateTime(2015, 05, 15, 10, 10, 10, DateTimeKind.Utc),
                              IsDeleted = false,
                              InterviewId = "1",
                              UserId = "1",
                              User = new ApplicationUser
                              {
                                Id = "1",
                                Email = "toni@toni.com",
                                UserName = "toni@toni.com",
                                FirstName = "Toni",
                                LastName = "Dimitrov",
                                IsDeleted = false,
                                Image = "avatar",
                              },
                         },
                    },
                },
                new Interview
                {
                    Id = "2",
                    Seniority = PositionSeniority.RegularDeveloper,
                    PositionTitle = "Regular developer",
                    PositionDescription = "Regular with some experience",
                    LocationType = LocationType.InOffice,
                    BasedPositionLocation = "London",
                    CreatedOn = new DateTime(2015, 06, 15, 10, 10, 10, DateTimeKind.Utc),
                    ModifiedOn = new DateTime(2015, 06, 15, 10, 10, 10, DateTimeKind.Utc),
                    CompanyNationality = "Bulgarian",
                    IsDeleted = false,
                    UserId = "2",
                    User = new ApplicationUser
                    {
                        IsDeleted = false,
                        Email = "tony@tony.com",
                        FirstName = "Tony",
                        LastName = "Dimitrov",
                        Id = "2",
                    },
                    Questions = new List<Question>
                    {
                        new Question
                        {
                            Id = "3",
                            Content = "Waht is Encapsulation?",
                            GivenAnswer = "Data hiding",
                            RankType = QuestionRankType.MostUnexpected,
                            IsDeleted = false,
                            CreatedOn = new DateTime(2015, 06, 15, 10, 10, 10, DateTimeKind.Utc),
                            ModifiedOn = new DateTime(2015, 06, 15, 10, 10, 10, DateTimeKind.Utc),
                            InterviewId = "1",
                        },
                        new Question
                        {
                            Id = "4",
                            Content = "Waht is Polymorphism?",
                            GivenAnswer = "Overrading methods",
                            RankType = QuestionRankType.MostInteresting,
                            IsDeleted = false,
                            CreatedOn = new DateTime(2015, 06, 15, 10, 10, 10, DateTimeKind.Utc),
                            ModifiedOn = new DateTime(2015, 06, 15, 10, 10, 10, DateTimeKind.Utc),
                            InterviewId = "1",
                            Comments = new List<Comment>
                            {
                                new Comment
                                {
                                Id = "1",
                                Content = "Not a difficult questions",
                                CreatedOn = new DateTime(2015, 06, 15, 10, 10, 10, DateTimeKind.Utc),
                                ModifiedOn = new DateTime(2015, 06, 15, 10, 10, 10, DateTimeKind.Utc),
                                IsDeleted = false,
                                QuestionId = "4",
                                InterviewId = "1",
                                UserId = "2",
                                },
                                new Comment
                                {
                                Id = "1",
                                Content = "Regular questions",
                                CreatedOn = new DateTime(2015, 06, 15, 10, 10, 10, DateTimeKind.Utc),
                                IsDeleted = true,
                                DeletedOn = new DateTime(2015, 06, 15, 10, 10, 10, DateTimeKind.Utc),
                                QuestionId = "4",
                                InterviewId = "1",
                                UserId = "2",
                                },
                            },
                        },
                    },
                    Comments = new List<Comment>
                    {
                        new Comment
                        {
                            Id = "1",
                            Content = "Very short interview",
                            CreatedOn = new DateTime(2015, 06, 15, 10, 10, 10, DateTimeKind.Utc),
                            IsDeleted = false,
                            InterviewId = "1",
                            UserId = "1",
                        },
                        new Comment
                        {
                            Id = "2",
                            Content = "not enough",
                            CreatedOn = new DateTime(2015, 06, 15, 10, 10, 10, DateTimeKind.Utc),
                            IsDeleted = true,
                            InterviewId = "1",
                            UserId = "1",
                        },
                    },
                    Likes = new List<Like>
                    {
                         new Like
                         {
                              Id = "3",
                              IsLiked = true,
                              CreatedOn = new DateTime(2015, 05, 15, 10, 10, 10, DateTimeKind.Utc),
                              IsDeleted = false,
                              InterviewId = "1",
                              UserId = "1",
                         },
                         new Like
                         {
                              Id = "4",
                              IsLiked = true,
                              CreatedOn = new DateTime(2015, 06, 15, 10, 10, 10, DateTimeKind.Utc),
                              IsDeleted = false,
                              InterviewId = "1",
                              UserId = "2",
                         },
                    },
                },
            };

            return data.AsQueryable<Interview>();
        }

        public static IEnumerable<InterviewVM> GetCountInterviewsTestData(int count)
        {
            var interviewsVM = new List<InterviewVM>();

            for (int i = 0; i < count; i++)
            {
                interviewsVM.Add(new InterviewVM
                {
                    InterviewId = i.ToString(),
                });
            }

            return interviewsVM;
        }

        public static async Task<Interview> GetInterviewById()
        {
            return await Task.Run(() =>
             {
                 var interviews = GetInterviewsTestData();

                 var interview = interviews.First();

                 return interview;
             });
        }

        public static IQueryable<Interview> GetInterviewWithCommentsTestData()
        {
            return new List<Interview>
            {
                new Interview
                {
                    Id = "1",
                    Seniority = PositionSeniority.JuniorDeveloper,
                    PositionTitle = "Junior with some experience",
                    PositionDescription = "Junior with some experience on .net core",
                    LocationType = LocationType.InOffice,
                    BasedPositionLocation = "Sofia",
                    CreatedOn = new DateTime(2016, 05, 15, 10, 10, 10, DateTimeKind.Utc),
                    CompanyNationality = "Bulgarian",
                    IsDeleted = false,
                    UserId = "1",
                    Comments = new List<Comment>
                    {
                        new Comment
                        {
                            Id = "1",
                            InterviewId = "1",
                            Content = "Very short interview",
                            CreatedOn = new DateTime(2015, 05, 15, 10, 10, 10, DateTimeKind.Utc),
                            UserId = "1",
                            IsDeleted = false,
                            User = new ApplicationUser
                            {
                                Id = "1",
                                FirstName = "Toni",
                                LastName = "Dimitrov",
                            },
                        },
                        new Comment
                        {
                            Id = "2",
                            InterviewId = "1",
                            Content = "not enough",
                            CreatedOn = new DateTime(2016, 05, 15, 10, 10, 10, DateTimeKind.Utc),
                            UserId = "2",
                            IsDeleted = false,
                            User = new ApplicationUser
                            {
                                Id = "2",
                                FirstName = "Ivan",
                            },
                        },
                    },
                    User = new ApplicationUser
                    {
                        Id = "1",
                        Email = "toni@toni.com",
                        UserName = "toni@toni.com",
                        FirstName = "Toni",
                        LastName = "Dimitrov",
                        IsDeleted = false,
                        Image = "avatar",
                    },
                },
            }.AsQueryable();
        }

        public static CreateInterviewVM CreateInterviewTestData()
        {
            return new CreateInterviewVM
            {
                Seniority = PositionSeniorityVM.JuniorDeveloper,
                PositionTitle = "Junior with some experience",
                PositionDescription = "Junior with some experience on .net core",
                LocationType = LocationType.InOffice.ToString(),
                BasedPositionLocation = "Sofia",
                CompanyNationality = "Bulgarian",
                Employees = EmployeesSizeVM.Between100And1000,
                Questions = new List<CreateInterviewQuestionVM>
                {
                    new CreateInterviewQuestionVM
                    {
                        Content = "Encapsulation",
                        GivenAnswer = "Data hiding",
                        Unexpected = (int)QuestionRankTypeVM.MostUnexpected,
                    },
                    new CreateInterviewQuestionVM
                    {
                        Content = "Polymorphism",
                        GivenAnswer = "Overriding",
                        Unexpected = (int)QuestionRankTypeVM.None,
                    },
                },
            };
        }
    }
}
