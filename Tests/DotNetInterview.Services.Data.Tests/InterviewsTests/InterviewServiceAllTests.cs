namespace DotNetInterview.Services.Data.Tests.InterviewsTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using DotNetInterview.Common;
    using DotNetInterview.Data.Common.Repositories;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Data.Models.Enums;
    using DotNetInterview.Web.ViewModels.Comments.DTO;
    using DotNetInterview.Web.ViewModels.Interviews;
    using Moq;
    using Xunit;

    public class InterviewServiceAllTests
    {
        [Fact]
        public async Task All_AllSeniorities_ReturnCorrectData()
        {
            var mockedData = InterviewsTestData.GetInterviewsTestData();
            mockedData.ToArray()[0].CreatedOn = new DateTime(2020, 05, 15, 12, 10, 10, DateTimeKind.Utc);
            mockedData.ToArray()[1].CreatedOn = new DateTime(2015, 06, 15, 12, 10, 10, DateTimeKind.Utc);

            var hourToLocal = mockedData.ToArray()[0].CreatedOn.ToLocalTime().Hour;

            var interviewRepo = new Mock<IDeletableEntityRepository<Interview>>();
            interviewRepo
                .Setup(r => r.All())
                .Returns(mockedData);

            var service = new InterviewsService(null, interviewRepo.Object, null, null, null, null);

            // Arrange
            var interviewsVM = await service.All(seniority: 0);

            // Act
            Assert.Equal(2, interviewsVM.Interviews.Count());

            Assert.Equal("1", interviewsVM.Interviews.ToArray()[0].InterviewId);
            Assert.Equal("Junior with some experience", interviewsVM.Interviews.ToArray()[0].PositionTitle);

            // Seniority is parsed in view with tag helper to => Junior developer
            Assert.Equal("JuniorDeveloper", interviewsVM.Interviews.ToArray()[0].Seniority);
            Assert.Equal($"15 May 2020 {hourToLocal:00}:10", interviewsVM.Interviews.ToArray()[0].Date);

            Assert.Equal(2, interviewsVM.Interviews.ToArray()[0].Questions);
            Assert.Equal(2, interviewsVM.Interviews.ToArray()[0].Likes);
            Assert.Equal("1", interviewsVM.Interviews.ToArray()[0].CreatorId);
            Assert.Equal("Toni D.", interviewsVM.Interviews.ToArray()[0].CreatorName);
            Assert.Equal(string.Empty, interviewsVM.Interviews.ToArray()[0].DisableUserLink);
            Assert.Equal("avatar", interviewsVM.Interviews.ToArray()[0].CreatorAvatar);

            // Second interview
            Assert.Equal("2", interviewsVM.Interviews.ToArray()[1].InterviewId);
            Assert.Equal("Regular developer", interviewsVM.Interviews.ToArray()[1].PositionTitle);

            // Seniority is parsed in view with tag helper to => Regular developer
            Assert.Equal("RegularDeveloper", interviewsVM.Interviews.ToArray()[1].Seniority);
            Assert.Equal($"15 Jun 2015 {hourToLocal:00}:10", interviewsVM.Interviews.ToArray()[1].Date);

            Assert.Equal(2, interviewsVM.Interviews.ToArray()[1].Questions);
            Assert.Equal(2, interviewsVM.Interviews.ToArray()[1].Likes);
            Assert.Equal(string.Empty, interviewsVM.Interviews.ToArray()[1].CreatorId);
            Assert.Equal("No user info", interviewsVM.Interviews.ToArray()[1].CreatorName);
            Assert.Equal(GlobalConstants.DisableLink, interviewsVM.Interviews.ToArray()[1].DisableUserLink);
            Assert.Null(interviewsVM.Interviews.ToArray()[1].CreatorAvatar);
        }

        [Fact]
        public async Task All_OnlyRegularDeveloperSeniority_ReturnCorrectData()
        {
            // Arrange
            var mockedData = InterviewsTestData.GetInterviewsTestData();
            int hour = 12;
            mockedData.ToArray()[0].CreatedOn = new DateTime(2015, 05, 15, hour, 10, 10, DateTimeKind.Utc);
            var localDifferentHoursFromUtc = DateTime.Now.Hour - DateTime.UtcNow.Hour;
            var formattedHour = hour + localDifferentHoursFromUtc;

            var interviewRepo = new Mock<IDeletableEntityRepository<Interview>>();
            interviewRepo.Setup(r => r.All())
                .Returns(mockedData);

            var service = new InterviewsService(null, interviewRepo.Object, null, null, null, null);

            // Act
            var interviewsVM = await service.All((int)PositionSeniority.RegularDeveloper);

            // Assert
            Assert.Single(interviewsVM.Interviews);
            Assert.Equal("RegularDeveloper", interviewsVM.Interviews.ToArray()[0].Seniority);
        }

        [Theory]
        [InlineData(0, 1, 0, 0, GlobalConstants.DisableLink, GlobalConstants.DisableLink)]
        [InlineData(2, 1, 2, 1, GlobalConstants.DisableLink, GlobalConstants.DisableLink)]
        [InlineData(10, 1, GlobalConstants.ResultsPerPage, 1, GlobalConstants.DisableLink, GlobalConstants.DisableLink)]
        [InlineData(31, 1, GlobalConstants.ResultsPerPage, 3, GlobalConstants.DisableLink, "")]
        [InlineData(31, 4, 1, 1, "", GlobalConstants.DisableLink)]
        [InlineData(25, 2, GlobalConstants.ResultsPerPage, 3, GlobalConstants.DisableLink, GlobalConstants.DisableLink)]
        public void AllByPage_AllSenioritiesByPage_ReturnCorrectDataPerPage(int collectionLength, int page, int resultPerPage, int paginationLength, string prevButton, string nextButton)
        {
            // Arrange
            var mockedData = InterviewsTestData.GetCountInterviewsTestData(collectionLength);
            var service = new InterviewsService(null, null, null, null, null, null);

            // Act
            var interviewsVM = service.AllByPage(page, new AllInterviewsVM(), mockedData);

            // Assert
            Assert.Equal(resultPerPage, interviewsVM.Interviews.Count());

            Assert.Equal(paginationLength, interviewsVM.PaginationLength);
            Assert.Equal(prevButton, interviewsVM.PrevtDisable);
            Assert.Equal(nextButton, interviewsVM.NextDisable);
        }

        [Fact]
        public void AllComments_AllWhenNotOwnerNorAdmin_ShouldReturnWithOptionToDelete()
        {
            // Arrange
            var mockedData = InterviewsTestData.GetInterviewsTestData();
            var interviewRepo = new Mock<IDeletableEntityRepository<Interview>>();
            interviewRepo.Setup(r => r.All())
                .Returns(mockedData);
            var service = new InterviewsService(null, interviewRepo.Object, null, null, null, null);
        }

        [Fact]
        public async Task AddComment_AddComment_ReturnCorrectCommentsCount()
        {
            // Arrange
            var mockedData = InterviewsTestData.GetInterviewsTestData();
            var interviewRepo = new Mock<IDeletableEntityRepository<Interview>>();
            interviewRepo.Setup(r => r.All())
                .Returns(mockedData);
            var service = new InterviewsService(null, interviewRepo.Object, null, null, null, null);
            var commentDTO = new AddCommentDTO
            {
                Id = "1",
                Content = "add 3",
            };
            var interviewCommnets = mockedData.FirstOrDefault(i => i.Id == commentDTO.Id).Comments.Count;

            // Act
            await service.AddComment(commentDTO, "1");

            // Arrange
            var interviewWithAddedComment = mockedData.FirstOrDefault(i => i.Id == commentDTO.Id);
            Assert.Equal(interviewCommnets + 1, interviewWithAddedComment.Comments.Count);
            Assert.Contains(interviewWithAddedComment.Comments, c => c.Content == "add 3");
        }
    }
}

//var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//.UseInMemoryDatabase(databaseName: "allInterviewsDatabase") // Give a Unique name to the DB
//.Options;

//var dbContext = new Mock<ApplicationDbContext>(options);

//var questionsRepo = new Mock<IDeletableEntityRepository<Question>>();
//var commentsRepo = new Mock<IDeletableEntityRepository<Comment>>();
//var likeRepo = new Mock<IDeletableEntityRepository<Like>>();
//var importerRepo = new Mock<IImporterHelperService>();