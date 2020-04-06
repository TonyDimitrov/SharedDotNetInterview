namespace DotNetInterview.Services.Data.Tests.HelperTests
{
    using System;

    using DotNetInterview.Common;
    using DotNetInterview.Services.Data.Helpers;
    using DotNetInterview.Web.ViewModels.Enums;
    using DotNetInterview.Web.ViewModels.Interviews.DTO;
    using Xunit;

    public class UtilsTests
    {
        [Fact]
        public void HideDelete_HideForNoAdminAndNoOwner_ShouldSetHidden()
        {
            // Arrange
            var resourceOwnerId = "rty456";
            var loggedinUserId = "qwe123";
            var isAdmin = false;

            // Act
            var hidden = Utils.HideDelete(resourceOwnerId, loggedinUserId, isAdmin);

            // Assert
            Assert.Equal(GlobalConstants.Hidden, hidden);
        }

        [Fact]
        public void HideDelete_ShowForNoAdminButOwner_ShouldNotSetHidden()
        {
            // Arrange
            var resourceOwnerAndLoggedInUserId = "rty456";
            var isAdmin = false;

            // Act
            var shown = Utils.HideDelete(resourceOwnerAndLoggedInUserId, resourceOwnerAndLoggedInUserId, isAdmin);

            // Assert
            Assert.Equal(string.Empty, shown);
        }

        [Fact]
        public void HideDelete_ShowForAdmiButNotOwner_ShouldNotSetHidden()
        {
            // Arrange
            var resourceOwnerId = "rty456";
            var loggedinUserId = "qwe123";
            var isAdmin = true;

            // Act
            var shown = Utils.HideDelete(resourceOwnerId, loggedinUserId, isAdmin);

            // Assert
            Assert.Equal(string.Empty, shown);
        }

        [Fact]
        public void HideAddComment_HedeForNotLoggedInUser_ShouldSetHidden()
        {
            // Arrange
            string notLoggedInUserId = null;

            // Act
            var hidden = Utils.HideAddComment(notLoggedInUserId);

            // Assert
            Assert.Equal(GlobalConstants.Hidden, hidden);
        }

        [Fact]
        public void HideAddComment_ShowForLoggedInUser_ShouldNotSetHidden()
        {
            // Arrange
            string loggedInUserId = "qwe123";

            // Act
            var shown = Utils.HideAddComment(loggedInUserId);

            // Assert
            Assert.Equal(string.Empty, shown);
        }

        [Fact]
        public void IsModified_EntityIsModified_ShouldReturnTrue()
        {
            // Arrange
            var createdOn = DateTime.Today;
            var modifiedOn = DateTime.Today.AddDays(5);

            // Act
            var isModified = Utils.IsModified(createdOn, modifiedOn);

            // Assert
            Assert.True(isModified);
        }

        [Fact]
        public void IsModified_EntityIsNotModified_ShouldReturnFalse()
        {
            // Arrange
            var createdOn = new DateTime(2015, 05, 15);
            DateTime? modifiedOn = null;

            // Act
            var isModified = Utils.IsModified(createdOn, modifiedOn);

            // Assert
            Assert.False(isModified);
        }

        [Fact]
        public void SetStringValues_HideWhenFieldIsNull_ShouldSetHidden()
        {
            // Arrange
            var model = new EditInterviewQuestionsDTO();

            // Act
            Utils.SetStringValues<EditInterviewQuestionsDTO>(model, null);

            // Assert
            Assert.Equal(GlobalConstants.Hidden, model.GivenAnswerCss);
            Assert.Equal(GlobalConstants.AddAnswer, model.GivenAnswerBtnText);
        }

        [Fact]
        public void SetStringValues_ShowWhenFieldHasText_ShouldNotSetHidden()
        {
            // Arrange
            var model = new EditInterviewQuestionsDTO();

            // Act
            Utils.SetStringValues<EditInterviewQuestionsDTO>(model, "Answer to question");

            // Assert
            Assert.Equal(string.Empty, model.GivenAnswerCss);
            Assert.Equal(GlobalConstants.DeleteAnswer, model.GivenAnswerBtnText);
        }

        [Fact]
        public void ParseEnum_SplitEnumValuesBygivenSeparator_ShouldSplit()
        {
            // Arrange
            var @enum = QuestionRankTypeVM.MostDifficult;

            // Act
            var splitValue = Utils.ParseEnum<QuestionRankTypeVM>(@enum, separator: " ");

            // Assert
            Assert.Equal("Most Difficult", splitValue);
        }
    }
}
