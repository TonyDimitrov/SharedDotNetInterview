namespace DotNetInterview.Services.Data.Tests.QuestionsTests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using DotNetInterview.Data;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Data.Repositories;
    using DotNetInterview.Services.Data.Tests.InterviewsTests;
    using DotNetInterview.Web.ViewModels.Comments;
    using DotNetInterview.Web.ViewModels.Comments.DTO;
    using DotNetInterview.Web.ViewModels.Enums;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class QuestionsServiceAllTests
    {
        [Fact]
        public async Task All_AllQuestions_AllReturned()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase("all_questions1");

            var interviewRepository = new EfDeletableEntityRepository<Interview>(new ApplicationDbContext(options.Options));
            var questionRepository = new EfDeletableEntityRepository<Question>(new ApplicationDbContext(options.Options));

            var fileService = new Mock<IFileService>();
            var fileMock = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");
            fileService.Setup(f => f.SaveFile(fileMock, "fileDirectory"))
                .ReturnsAsync("fileForInterviewQuestion");

            var interviewService = new InterviewsService(null, interviewRepository, null, null, null, null);
            var newInterview = InterviewsTestData.CreateInterviewTestData();
            newInterview.Questions[0].FormFile = fileMock;
            await interviewService.Create(newInterview, "1", "fileDirectory", fileService.Object);

            var questionService = new QuestionsService(questionRepository);

            // Act
            var questions = questionService.All(4, "1", false);

            // Assert
            Assert.Equal(2, questions.Questions.Count());

            // Questions are ordered by date of creation
            Assert.True(questions.Questions.First().HideRanked);
            Assert.False(questions.Questions.Last().HideRanked);
        }

        [Fact]
        public async Task All_MostUnexpectedlQuestions_AllReturned()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase("all_questions2");

            var interviewRepository = new EfDeletableEntityRepository<Interview>(new ApplicationDbContext(options.Options));
            var questionRepository = new EfDeletableEntityRepository<Question>(new ApplicationDbContext(options.Options));

            var fileService = new Mock<IFileService>();
            var fileMock = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");
            fileService.Setup(f => f.SaveFile(fileMock, "fileDirectory"))
                .ReturnsAsync("fileForInterviewQuestion");

            var interviewService = new InterviewsService(null, interviewRepository, null, null, null, null);
            var newInterview = InterviewsTestData.CreateInterviewTestData();
            newInterview.Questions[0].FormFile = fileMock;
            await interviewService.Create(newInterview, "1", "fileDirectory", fileService.Object);

            var questionService = new QuestionsService(questionRepository);

            // Act
            var questions = questionService.All((int)QuestionRankTypeVM.MostUnexpected, "1", false);

            // Assert
            Assert.Single(questions.Questions);
            Assert.False(questions.Questions.First().HideRanked);
        }

        [Fact]
        public async Task AllComments_AllAddedQuestionCooments_AllReturned()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase("all_questions2");

            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

            var dumyUser = new ApplicationUser
            {
                Email = "toni@toni.com",
                PasswordHash = "AQAAAAEAACcQAAAAEDt5MojrolghU7VyfjhjsjX52RaGxtaTa0/n9LXcQ8gL54ihwg6UEcdkMj0ckE4jJw==",
                UserName = "toni@toni.com",
                FirstName = "Toni",
                LastName = "Dimitrov",
                IsDeleted = false,
                Image = "avatar",
            };

            await userRepository.AddAsync(dumyUser);
            await userRepository.SaveChangesAsync();

            var user = userRepository.All().ToArray().First();

            var interviewRepository = new EfDeletableEntityRepository<Interview>(new ApplicationDbContext(options.Options));
            var questionRepository = new EfDeletableEntityRepository<Question>(new ApplicationDbContext(options.Options));

            var fileService = new Mock<IFileService>();
            var fileMock = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");
            fileService.Setup(f => f.SaveFile(fileMock, "fileDirectory"))
                .ReturnsAsync("fileForInterviewQuestion");

            var interviewService = new InterviewsService(null, interviewRepository, null, null, null, null);
            var newInterview = InterviewsTestData.CreateInterviewTestData();
            newInterview.Questions[0].FormFile = fileMock;
            await interviewService.Create(newInterview, "1", "fileDirectory", fileService.Object);
            var questionService = new QuestionsService(questionRepository);
            var questionsId = questionService.All((int)QuestionRankTypeVM.MostUnexpected, "1", false)
                .Questions
                .FirstOrDefault()
                .QuestionId;

            // Act
            await questionService.AddComment(new AddCommentDTO { Id = questionsId, Content = "hello", }, user.Id);
            await questionService.AddComment(new AddCommentDTO { Id = questionsId, Content = "hello there", }, user.Id);
            var comments = questionService.AllComments<IEnumerable<AllCommentsVM>>(questionsId, user.Id, false);

            // Assert
            Assert.Equal(2, comments.Count());
            Assert.Equal("hello", comments.First().Content);
            Assert.Equal(string.Empty, comments.First().HideDelete);
            Assert.Equal("hello there", comments.Last().Content);
            Assert.Equal(string.Empty, comments.Last().HideDelete);
        }
    }
}
