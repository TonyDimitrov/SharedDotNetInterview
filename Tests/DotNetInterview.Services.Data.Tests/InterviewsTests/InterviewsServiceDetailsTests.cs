namespace DotNetInterview.Services.Data.Tests.InterviewsTests
{
    using System.Linq;

    using DotNetInterview.Common;
    using DotNetInterview.Data.Common.Repositories;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Data.Models.Enums;
    using Moq;
    using Xunit;

    public class InterviewsServiceDetailsTests
    {
        [Fact]
        public void Details_GetDetailsWhenNotInterviewOwnerNorAdmin_ReturnCorrectDetails()
        {
            // Arrange
            var interviewsRepo = new Mock<IDeletableEntityRepository<Interview>>();
            var interviews = InterviewsTestData.GetInterviewsTestData();
            interviewsRepo.Setup(r => r.All()).Returns(interviews);

            var service = new InterviewsService(null, interviewsRepo.Object, null, null, null, null);

            // Act
            var interviewVM = service.Details("1", "not existing id", false);

            // Assert
            Assert.Equal("Junior with some experience", interviewVM.PositionTitle);
            Assert.Equal(PositionSeniority.JuniorDeveloper.ToString(), interviewVM.Seniority);
            Assert.Equal("Bulgarian", interviewVM.CompanyNationality);
            Assert.Equal(EmployeesSize.Between100And1000.ToString(), interviewVM.CompanySize);
            Assert.Equal(LocationType.InOffice.ToString(), interviewVM.LocationType);
            Assert.Equal("Junior with some experience on .net core", interviewVM.PositionDescription);
            Assert.Equal("Sofia", interviewVM.BasedPositionLocation);
            Assert.NotNull(interviewVM.CreatedOn);
            Assert.Equal("Toni D.", interviewVM.UserFullName);
            Assert.Equal(string.Empty, interviewVM.AddLike);
            Assert.Equal(GlobalConstants.Hidden, interviewVM.CanDelete);
            Assert.Equal(GlobalConstants.Hidden, interviewVM.CanEdit);
            Assert.Equal(GlobalConstants.Hidden, interviewVM.CanHardDelete);

            Assert.Equal("Waht is Encapsulation?", interviewVM.InterviewQns.ToArray()[0].Content);
            Assert.Equal("Data hiding", interviewVM.InterviewQns.ToArray()[0].Answer);
            Assert.Equal(QuestionRankType.MostInteresting.ToString(), interviewVM.InterviewQns.ToArray()[1].Ranked);
            Assert.NotNull(interviewVM.InterviewQns.ToArray()[0].File);
            Assert.False(interviewVM.InterviewQns.ToArray()[0].HideAnswer);
            Assert.False(interviewVM.InterviewQns.ToArray()[0].HideRanked);
            Assert.False(interviewVM.InterviewQns.ToArray()[0].HideFile);

            Assert.Equal("Waht is Polymorphism?", interviewVM.InterviewQns.ToArray()[1].Content);
            Assert.Equal("Overrading methods", interviewVM.InterviewQns.ToArray()[1].Answer);
            Assert.Equal(QuestionRankType.MostInteresting.ToString(), interviewVM.InterviewQns.ToArray()[1].Ranked);
            Assert.Null(interviewVM.InterviewQns.ToArray()[1].File);
            Assert.False(interviewVM.InterviewQns.ToArray()[1].HideAnswer);
            Assert.False(interviewVM.InterviewQns.ToArray()[1].HideRanked);
            Assert.True(interviewVM.InterviewQns.ToArray()[1].HideFile);

            Assert.Equal(2, interviewVM.InterviewQns.ToArray()[1].QnsComments.Count());
            Assert.Equal("Not a difficult questions", interviewVM.InterviewQns.ToArray()[1].QnsComments.ToArray()[0].Content);
            Assert.Equal(GlobalConstants.Hidden, interviewVM.InterviewQns.ToArray()[1].QnsComments.ToArray()[0].HideDelete);
            Assert.Equal(string.Empty, interviewVM.InterviewQns.ToArray()[1].QnsComments.ToArray()[0].HideAdd);

            Assert.Equal(2, interviewVM.Likes);
        }

        [Fact]
        public void Details_GetDetailsWhenInterviewOwnerButNotAdmin_ReturnCorrectDetails()
        {
            // Arrange
            var interviewsRepo = new Mock<IDeletableEntityRepository<Interview>>();
            var interviews = InterviewsTestData.GetInterviewsTestData();
            interviewsRepo.Setup(r => r.All()).Returns(interviews);

            var service = new InterviewsService(null, interviewsRepo.Object, null, null, null, null);

            // Act
            var interviewVM = service.Details("1", "1", false);

            // Assert
            Assert.Equal("Junior with some experience", interviewVM.PositionTitle);
            Assert.Equal(PositionSeniority.JuniorDeveloper.ToString(), interviewVM.Seniority);
            Assert.Equal("Bulgarian", interviewVM.CompanyNationality);
            Assert.Equal(EmployeesSize.Between100And1000.ToString(), interviewVM.CompanySize);
            Assert.Equal(LocationType.InOffice.ToString(), interviewVM.LocationType);
            Assert.Equal("Junior with some experience on .net core", interviewVM.PositionDescription);
            Assert.Equal("Sofia", interviewVM.BasedPositionLocation);
            Assert.NotNull(interviewVM.CreatedOn);
            Assert.Equal("Toni D.", interviewVM.UserFullName);
            Assert.Equal(GlobalConstants.LikedCss, interviewVM.AddLike);
            Assert.Equal(string.Empty, interviewVM.CanDelete);
            Assert.Equal(string.Empty, interviewVM.CanEdit);
            Assert.Equal(GlobalConstants.Hidden, interviewVM.CanHardDelete);

            Assert.Equal("Waht is Encapsulation?", interviewVM.InterviewQns.ToArray()[0].Content);
            Assert.Equal("Data hiding", interviewVM.InterviewQns.ToArray()[0].Answer);
            Assert.Equal(QuestionRankType.MostInteresting.ToString(), interviewVM.InterviewQns.ToArray()[1].Ranked);
            Assert.NotNull(interviewVM.InterviewQns.ToArray()[0].File);
            Assert.False(interviewVM.InterviewQns.ToArray()[0].HideAnswer);
            Assert.False(interviewVM.InterviewQns.ToArray()[0].HideRanked);
            Assert.False(interviewVM.InterviewQns.ToArray()[0].HideFile);

            Assert.Equal("Waht is Polymorphism?", interviewVM.InterviewQns.ToArray()[1].Content);
            Assert.Equal("Overrading methods", interviewVM.InterviewQns.ToArray()[1].Answer);
            Assert.Equal(QuestionRankType.MostInteresting.ToString(), interviewVM.InterviewQns.ToArray()[1].Ranked);
            Assert.Null(interviewVM.InterviewQns.ToArray()[1].File);
            Assert.False(interviewVM.InterviewQns.ToArray()[1].HideAnswer);
            Assert.False(interviewVM.InterviewQns.ToArray()[1].HideRanked);
            Assert.True(interviewVM.InterviewQns.ToArray()[1].HideFile);

            Assert.Equal(2, interviewVM.InterviewQns.ToArray()[1].QnsComments.Count());
            Assert.Equal("Not a difficult questions", interviewVM.InterviewQns.ToArray()[1].QnsComments.ToArray()[0].Content);
            Assert.Equal(string.Empty, interviewVM.InterviewQns.ToArray()[1].QnsComments.ToArray()[0].HideDelete);
            Assert.Equal(string.Empty, interviewVM.InterviewQns.ToArray()[1].QnsComments.ToArray()[0].HideAdd);

            Assert.Equal(2, interviewVM.Likes);
        }

        [Fact]
        public void Details_GetDetailsWhenNotInterviewOwnerButAdmin_ReturnCorrectDetails()
        {
            // Arrange
            var interviewsRepo = new Mock<IDeletableEntityRepository<Interview>>();
            var interviews = InterviewsTestData.GetInterviewsTestData();
            interviewsRepo.Setup(r => r.All()).Returns(interviews);

            var service = new InterviewsService(null, interviewsRepo.Object, null, null, null, null);

            // Act
            var interviewVM = service.Details("1", "not existing id", true);

            // Assert
            Assert.Equal("Junior with some experience", interviewVM.PositionTitle);
            Assert.Equal(PositionSeniority.JuniorDeveloper.ToString(), interviewVM.Seniority);
            Assert.Equal("Bulgarian", interviewVM.CompanyNationality);
            Assert.Equal(EmployeesSize.Between100And1000.ToString(), interviewVM.CompanySize);
            Assert.Equal(LocationType.InOffice.ToString(), interviewVM.LocationType);
            Assert.Equal("Junior with some experience on .net core", interviewVM.PositionDescription);
            Assert.Equal("Sofia", interviewVM.BasedPositionLocation);
            Assert.NotNull(interviewVM.CreatedOn);
            Assert.Equal("Toni D.", interviewVM.UserFullName);
            Assert.Equal(string.Empty, interviewVM.AddLike);
            Assert.Equal(string.Empty, interviewVM.CanDelete);
            Assert.Equal(GlobalConstants.Hidden, interviewVM.CanEdit);
            Assert.Equal(string.Empty, interviewVM.CanHardDelete);

            Assert.Equal("Waht is Encapsulation?", interviewVM.InterviewQns.ToArray()[0].Content);
            Assert.Equal("Data hiding", interviewVM.InterviewQns.ToArray()[0].Answer);
            Assert.Equal(QuestionRankType.MostInteresting.ToString(), interviewVM.InterviewQns.ToArray()[1].Ranked);
            Assert.NotNull(interviewVM.InterviewQns.ToArray()[0].File);
            Assert.False(interviewVM.InterviewQns.ToArray()[0].HideAnswer);
            Assert.False(interviewVM.InterviewQns.ToArray()[0].HideRanked);
            Assert.False(interviewVM.InterviewQns.ToArray()[0].HideFile);

            Assert.Equal("Waht is Polymorphism?", interviewVM.InterviewQns.ToArray()[1].Content);
            Assert.Equal("Overrading methods", interviewVM.InterviewQns.ToArray()[1].Answer);
            Assert.Equal(QuestionRankType.MostInteresting.ToString(), interviewVM.InterviewQns.ToArray()[1].Ranked);
            Assert.Null(interviewVM.InterviewQns.ToArray()[1].File);
            Assert.False(interviewVM.InterviewQns.ToArray()[1].HideAnswer);
            Assert.False(interviewVM.InterviewQns.ToArray()[1].HideRanked);
            Assert.True(interviewVM.InterviewQns.ToArray()[1].HideFile);

            Assert.Equal(2, interviewVM.InterviewQns.ToArray()[1].QnsComments.Count());
            Assert.Equal("Not a difficult questions", interviewVM.InterviewQns.ToArray()[1].QnsComments.ToArray()[0].Content);
            Assert.Equal(string.Empty, interviewVM.InterviewQns.ToArray()[1].QnsComments.ToArray()[0].HideDelete);
            Assert.Equal(string.Empty, interviewVM.InterviewQns.ToArray()[1].QnsComments.ToArray()[0].HideAdd);

            Assert.Equal(2, interviewVM.Likes);
        }
    }
}
