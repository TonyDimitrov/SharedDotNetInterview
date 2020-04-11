namespace DotNetInterview.Services.Data.Tests.InterviewsTests
{
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using DotNetInterview.Data;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Data.Repositories;
    using DotNetInterview.Web.ViewModels.Comments.DTO;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class InterviewsServiceCreateTests
    {
        [Fact]
        public void CreateGetVM_GetModelForView_ReturnMode()
        {
            // Arrange
            var service = new InterviewsService(null, null, null, null, null, null);

            // Act
            var viewModel = service.CreateGetVM();

            // Assert
            Assert.Single(viewModel.Questions);
        }

        [Fact]
        public async Task Create_CreateInterview_StoreCorrectData()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("add_interview");
            var interviewRepository = new EfDeletableEntityRepository<Interview>(new ApplicationDbContext(options.Options));

            var fileService = new Mock<IFileService>();
            var fileMock = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");
            fileService.Setup(f => f.SaveFile(fileMock, "fileDirectory"))
                .ReturnsAsync("fileForInterviewQuestion");

            var service = new InterviewsService(null, interviewRepository, null, null, null, null);
            var newInterview = InterviewsTestData.CreateInterviewTestData();
            newInterview.Questions[0].FormFile = fileMock;

            // Act
            await service.Create(newInterview, "1", "fileDirectory", fileService.Object);
            var interviews = interviewRepository.All().ToList();
            var createdInterview = interviews.First();

            // Assert
            Assert.Single(interviews);
            Assert.Equal(newInterview.PositionTitle, createdInterview.PositionTitle);
            Assert.Equal(newInterview.Seniority.ToString(), createdInterview.Seniority.ToString());
            Assert.Equal(newInterview.CompanyNationality, createdInterview.CompanyNationality);
            Assert.Equal(newInterview.Employees.ToString(), createdInterview.Employees.ToString());
            Assert.Equal(newInterview.LocationType.ToString(), createdInterview.LocationType.ToString());
            Assert.Equal(newInterview.PositionDescription, createdInterview.PositionDescription);
            Assert.Equal(newInterview.BasedPositionLocation, createdInterview.BasedPositionLocation);

            Assert.Equal(newInterview.Questions[0].Content, createdInterview.Questions.ToArray()[0].Content);
            Assert.Equal(newInterview.Questions[0].GivenAnswer, createdInterview.Questions.ToArray()[0].GivenAnswer);
            Assert.Equal(newInterview.Questions[0].Unexpected, (int)createdInterview.Questions.ToArray()[0].RankType);
            Assert.Equal("fileForInterviewQuestion", createdInterview.Questions.ToArray()[0].UrlTask);

            Assert.Equal(newInterview.Questions[1].Content, createdInterview.Questions.ToArray()[1].Content);
            Assert.Equal(newInterview.Questions[1].GivenAnswer, createdInterview.Questions.ToArray()[1].GivenAnswer);
            Assert.Equal(0, (int)createdInterview.Questions.ToArray()[1].RankType);
            Assert.Null(createdInterview.Questions.ToArray()[1].UrlTask);
        }

        [Fact]
        public async Task AddComment_AddComment_ReturnCorrectCommentsCount()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase("add_comment");
            var interviewRepository = new EfDeletableEntityRepository<Interview>(new ApplicationDbContext(options.Options));

            var service = new InterviewsService(null, interviewRepository, null, null, null, null);
            var newInterview = InterviewsTestData.CreateInterviewTestData();

            var fileService = new Mock<IFileService>();
            fileService.Setup(f => f.SaveFile(null, "fileDirectory"))
                .Returns(Task.FromResult("FileForUser"));

            await service.Create(newInterview, "1", "file_derectotry", fileService.Object);

            var createdInterview = interviewRepository.All().FirstOrDefault();
            var comments = createdInterview.Comments.Count;
            var commentDTO = new AddCommentDTO
            {
                Id = createdInterview.Id,
                Content = "add comment",
            };

            // Act
            await service.AddComment(commentDTO, "1");

            // Arrange
            Assert.Equal(comments + 1, createdInterview.Comments.Count);
            Assert.Equal(commentDTO.Content, createdInterview.Comments.First().Content);
        }
    }
}
