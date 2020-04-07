namespace DotNetInterview.Services.Data.Tests.InterviewsTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using DotNetInterview.Common;
    using DotNetInterview.Data.Common.Repositories;
    using DotNetInterview.Data.Models;
    using Moq;
    using Xunit;

    public class InterviewServiceTests
    {
        [Fact]
        public async Task All_AllSeniorities_ReturnCorrectData()
        {
            var mockedData = new InterviewsTestData().GetInterviewsTestData();
            int hour = 12;
            mockedData.ToArray()[0].CreatedOn = new DateTime(2020, 05, 15, hour, 10, 10, DateTimeKind.Utc);
            var localDifferentHoursFromUtc = DateTime.Now.Hour - DateTime.UtcNow.Hour;
            var formattedHour = hour + localDifferentHoursFromUtc;

            var interviewRepo = new Mock<IDeletableEntityRepository<Interview>>();
            interviewRepo.Setup(r => r.All())
                .Returns(mockedData);

            IInterviewsService service = new InterviewsService(null, interviewRepo.Object, null, null, null, null);

            // Arrange
            var interviewsVM = await service.All(0);

            // Act
            Assert.Equal(2, interviewsVM.Interviews.Count());

            Assert.Equal("1", interviewsVM.Interviews.ToArray()[0].InterviewId);
            Assert.Equal("Junior with some experience", interviewsVM.Interviews.ToArray()[0].PositionTitle);
            Assert.Equal("Junior Developer", interviewsVM.Interviews.ToArray()[0].Seniority);
            Assert.Equal($"15 May 2015 {formattedHour:00}:16", interviewsVM.Interviews.ToArray()[0].Date);

            Assert.Equal(2, interviewsVM.Interviews.ToArray()[0].Questions);
            Assert.Equal(2, interviewsVM.Interviews.ToArray()[0].Likes);
            Assert.Equal("1", interviewsVM.Interviews.ToArray()[0].CreatorId);
            Assert.Equal("Toni", interviewsVM.Interviews.ToArray()[0].CreatorName);
            Assert.Equal(GlobalConstants.DesableLink, interviewsVM.Interviews.ToArray()[0].DisableUserLink);
            Assert.Equal("avatar", interviewsVM.Interviews.ToArray()[0].CreatorAvatar);
        }

       // [Fact]
        public void All_OnlyRegularDeveloperSeniority_ReturnCorrectData()
        {
            //var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            //.UseInMemoryDatabase(databaseName: "allInterviewsDatabase") // Give a Unique name to the DB
            //.Options;

            //var dbContext = new Mock<ApplicationDbContext>(options);

            // Assert            
            var mockedData = new InterviewsTestData().GetInterviewsTestData();
            int hour = 12;
            mockedData.ToArray()[0].CreatedOn = new DateTime(2015, 05, 15, hour, 10, 10, DateTimeKind.Utc);
            var localDifferentHoursFromUtc = DateTime.Now.Hour - DateTime.UtcNow.Hour;
            var formattedHour = hour + localDifferentHoursFromUtc;

            var interviewRepo = new Mock<IDeletableEntityRepository<Interview>>();
            interviewRepo.Setup(r => r.All())
                .Returns(mockedData);

            IInterviewsService service = new InterviewsService(null, interviewRepo.Object, null, null, null, null);

            //var questionsRepo = new Mock<IDeletableEntityRepository<Question>>();
            //var commentsRepo = new Mock<IDeletableEntityRepository<Comment>>();
            //var likeRepo = new Mock<IDeletableEntityRepository<Like>>();
            //var importerRepo = new Mock<IImporterHelperService>();
        }
    }
}
