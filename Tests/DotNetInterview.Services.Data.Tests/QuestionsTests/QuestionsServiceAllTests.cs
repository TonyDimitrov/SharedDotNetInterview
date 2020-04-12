namespace DotNetInterview.Services.Data.Tests.QuestionsTests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DotNetInterview.Common;
    using DotNetInterview.Data;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Data.Repositories;
    using DotNetInterview.Services.Data.Tests.InterviewsTests;
    using DotNetInterview.Services.Data.Tests.UsersTests;
    using DotNetInterview.Web.ViewModels.Comments;
    using DotNetInterview.Web.ViewModels.Comments.DTO;
    using DotNetInterview.Web.ViewModels.Enums;
    using DotNetInterview.Web.ViewModels.Questions;
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
        public async Task AllByPage_MostUnexpectedlQuestionsWhenNotLoggedIn_ReturnCorrectQuestionsAndHideAddForComment()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase("all_questions3");

            var interviewRepository = new EfDeletableEntityRepository<Interview>(new ApplicationDbContext(options.Options));
            var questionRepository = new EfDeletableEntityRepository<Question>(new ApplicationDbContext(options.Options));
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

            var dummyUser = UserTestData.GetUserTestData();
            await userRepository.AddAsync(dummyUser);
            await userRepository.SaveChangesAsync();
            var user = userRepository.All().ToArray().First();

            var fileService = new Mock<IFileService>();
            var fileMock = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");
            fileService.Setup(f => f.SaveFile(fileMock, "fileDirectory"))
                .ReturnsAsync("fileForInterviewQuestion");

            var interviewService = new InterviewsService(null, interviewRepository, null, null, null, null);
            var newInterview = InterviewsTestData.CreateInterviewTestData();
            newInterview.Questions[0].FormFile = fileMock;
            await interviewService.Create(newInterview, "1", "fileDirectory", fileService.Object);
            var questionService = new QuestionsService(questionRepository);

            var questionId = questionService.All((int)QuestionRankTypeVM.MostUnexpected, "1", false)
             .Questions
             .FirstOrDefault()
             .QuestionId;

            await questionService.AddComment(new AddCommentDTO { Id = questionId, Content = "hello", }, user.Id);
            await questionService.AddComment(new AddCommentDTO { Id = questionId, Content = "hello there", }, user.Id);

            // Act
            var questions = questionService.All((int)QuestionRankTypeVM.MostUnexpected, null, false);
            var questionsByPage = questionService.AllByPage(
                page: 1,
                new AllIQuestionsVM((int)QuestionRankTypeVM.MostUnexpected, GlobalConstants.Hidden),
                questions.Questions);

            // Assert
            Assert.Single(questionsByPage.Questions);
            Assert.Equal(2, questionsByPage.Questions.First().QnsComments.Count());
            Assert.False(questionsByPage.Questions.First().HideAnswer);
            Assert.False(questionsByPage.Questions.First().HideFile);
            Assert.False(questionsByPage.Questions.First().HideRanked);
            Assert.Equal(GlobalConstants.Hidden, questionsByPage.HideAddComment);
            Assert.True(questionsByPage.Questions.First().QnsComments.All(c => c.HideDelete == GlobalConstants.Hidden));
        }

        [Fact]
        public async Task AllComments_AllAddedQuestionComents_AllReturned()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase("all_questions4");

            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));
            var dummyUser = UserTestData.GetUserTestData();

            await userRepository.AddAsync(dummyUser);
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
            var questionId = questionService.All((int)QuestionRankTypeVM.MostUnexpected, "1", false)
                .Questions
                .FirstOrDefault()
                .QuestionId;

            // Act
            await questionService.AddComment(new AddCommentDTO { Id = questionId, Content = "hello", }, user.Id);
            await questionService.AddComment(new AddCommentDTO { Id = questionId, Content = "hello there", }, user.Id);
            var comments = questionService.AllComments<IEnumerable<AllCommentsVM>>(questionId, user.Id, false);

            // Assert
            Assert.Equal(2, comments.Count());
            Assert.Equal("hello", comments.First().Content);
            Assert.Equal(string.Empty, comments.First().HideDelete);
            Assert.Equal("hello there", comments.Last().Content);
            Assert.Equal(string.Empty, comments.Last().HideDelete);
        }
    }
}
