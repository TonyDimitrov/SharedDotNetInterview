namespace DotNetInterview.Services.Data.Tests.InterviewsTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DotNetInterview.Common;
    using DotNetInterview.Data.Common.Repositories;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Data.Models.Enums;
    using DotNetInterview.Web.ViewModels.Comments;
    using DotNetInterview.Web.ViewModels.Interviews;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Moq;
    using Xunit;

    public class InterviewsServiceAllTests
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

            var nalionalitiesService = new Mock<INationalitiesService>();
            nalionalitiesService.Setup(s => s.GetAll())
                .ReturnsAsync(new List<SelectListItem>
                {
                    new SelectListItem { Value = "Bulgaria", Text = "Bulgaria" },
                    new SelectListItem { Value = "US", Text = "US" },
                });

            var service = new InterviewsService(null, interviewRepo.Object, null, null, null, nalionalitiesService.Object);

            // Arrange
            var interviewsVM = await service.All(seniority: 0);

            // Act
            Assert.Equal(2, interviewsVM.Interviews.Count());

            Assert.Equal("1", interviewsVM.Interviews.ToArray()[0].InterviewId);
            Assert.Equal("Junior with some experience", interviewsVM.Interviews.ToArray()[0].PositionTitle);

            // Seniority is parsed in view with tag helper to => Junior developer
            Assert.Equal("jun-dev.png", interviewsVM.Interviews.ToArray()[0].Seniority);
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
            Assert.Equal("mid-dev.png", interviewsVM.Interviews.ToArray()[1].Seniority);
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

            var nalionalitiesService = new Mock<INationalitiesService>();
            nalionalitiesService.Setup(s => s.GetAll())
                .ReturnsAsync(new List<SelectListItem>
                {
                    new SelectListItem { Value = "Bulgaria", Text = "Bulgaria" },
                    new SelectListItem { Value = "US", Text = "US" },
                });

            var service = new InterviewsService(null, interviewRepo.Object, null, null, null, nalionalitiesService.Object);

            // Act
            var interviewsVM = await service.All((int)PositionSeniority.RegularDeveloper);

            // Assert
            Assert.Single(interviewsVM.Interviews);
            Assert.Equal("mid-dev.png", interviewsVM.Interviews.ToArray()[0].Seniority);
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
            var interviewsVM = service.AllByPage(page, new AllInterviewsVM(0), mockedData);

            // Assert
            Assert.Equal(resultPerPage, interviewsVM.Interviews.Count());

            Assert.Equal(paginationLength, interviewsVM.PaginationLength);
            Assert.Equal(prevButton, interviewsVM.PrevtDisable);
            Assert.Equal(nextButton, interviewsVM.NextDisable);
        }

        [Theory]
        [InlineData("1", "1", false)]
        public void AllComments_AllWhenOwnerNorAdmin_ShouldReturnWithOptionToDelete(string interviewId, string currentUserId, bool isAdmin)
        {
            // Arrange
            var mockedData = InterviewsTestData.GetInterviewWithCommentsTestData();
            var interviewRepo = new Mock<IDeletableEntityRepository<Interview>>();
            interviewRepo.Setup(r => r.All())
                .Returns(mockedData);
            var service = new InterviewsService(null, interviewRepo.Object, null, null, null, null);

            // Act
            var comments = service.AllComments<IEnumerable<AllCommentsVM>>(interviewId, currentUserId, isAdmin)
                .OrderBy(c => c.CommentId);

            var firstComment = comments.First();
            var secondComment = comments.Last();

            // Assert
            Assert.Equal(2, comments.Count());

            Assert.Equal(string.Empty, firstComment.HideAdd);
            Assert.Equal(string.Empty, firstComment.HideDelete);

            Assert.Equal(string.Empty, secondComment.HideAdd);
            Assert.Equal(GlobalConstants.Hidden, secondComment.HideDelete);
        }

        [Theory]
        [InlineData("1", "1", true)]
        [InlineData("1", "5", true)]
        public void AllComments_AllWhenAdmin_ShouldReturnWithOptionToDelete(string interviewId, string currentUserId, bool isAdmin)
        {
            // Arrange
            var mockedData = InterviewsTestData.GetInterviewWithCommentsTestData();
            var interviewRepo = new Mock<IDeletableEntityRepository<Interview>>();
            interviewRepo.Setup(r => r.All())
                .Returns(mockedData);
            var service = new InterviewsService(null, interviewRepo.Object, null, null, null, null);

            // Act
            var comments = service.AllComments<IEnumerable<AllCommentsVM>>(interviewId, currentUserId, isAdmin)
                .OrderBy(c => c.CommentId);

            var firstComment = comments.First();
            var secondComment = comments.Last();

            // Assert
            Assert.Equal(2, comments.Count());

            Assert.Equal(string.Empty, firstComment.HideAdd);
            Assert.Equal(string.Empty, firstComment.HideDelete);

            Assert.Equal(string.Empty, secondComment.HideAdd);
            Assert.Equal(string.Empty, secondComment.HideDelete);
        }

        [Theory]
        [InlineData("1", null, false)]
        public void AllComments_AllWhenNotLoggedIn_ShouldReturnWithWithoutOptionToDeleteAndAdd(string interviewId, string currentUserId, bool isAdmin)
        {
            // Arrange
            var mockedData = InterviewsTestData.GetInterviewWithCommentsTestData();
            var interviewRepo = new Mock<IDeletableEntityRepository<Interview>>();
            interviewRepo.Setup(r => r.All())
                .Returns(mockedData);
            var service = new InterviewsService(null, interviewRepo.Object, null, null, null, null);

            // Act
            var comments = service.AllComments<IEnumerable<AllCommentsVM>>(interviewId, currentUserId, isAdmin)
                .OrderBy(c => c.CommentId);

            var firstComment = comments.First();
            var secondComment = comments.Last();

            // Assert
            Assert.Equal(2, comments.Count());

            Assert.Equal(GlobalConstants.Hidden, firstComment.HideAdd);
            Assert.Equal(GlobalConstants.Hidden, firstComment.HideDelete);

            Assert.Equal(GlobalConstants.Hidden, secondComment.HideAdd);
            Assert.Equal(GlobalConstants.Hidden, secondComment.HideDelete);
        }
    }
}
