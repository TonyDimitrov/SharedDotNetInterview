namespace DotNetInterview.Services.Data.Tests.ExtensionTests
{
    using DotNetInterview.Services.Data.Extensions;
    using Xunit;

    public class StringExtensionTests
    {
        [Fact]
        public void FullUserNameParser_TakeOnlyFirstName_ShouldReturnCorrectFullName()
        {
            // Arrange
            var firstName = "Tonyyyyyyyyyyyyyyyyyyyyyyyy";
            var lastName = "Dimitrov";

            // Act
            var fullName = firstName.FullUserNameParser(lastName);

            // Assert
            Assert.Equal(20, fullName.Length);
            Assert.EndsWith("...", fullName);
        }

        [Fact]
        public void FullUserNameParser_TakeFirstNameAndSecondName_ShouldReturnCorrectFullName()
        {
            // Arrange
            var firstName = "Tony";
            var lastName = "Dimitrov";

            // Act
            var fullName = firstName.FullUserNameParser(lastName);

            // Assert
            Assert.Equal("Tony D.", fullName);
        }

        [Fact]
        public void SanitizeTextInput_ShoudRemoveHarmfullElements_ReturnSanitizedText()
        {
            // Arrange
            var text = "<p>hello</p>";
            var harmfullText = "<script> alert('danger');</script>";

            // Act
            var sanitizedText = string.Format("{0}{1}", text, harmfullText).SanitizeTextInput();

            // Assert
            Assert.Equal(text, sanitizedText);
        }

        [Fact]
        public void PositionTitleParser_TakeOnlyMaxLengthFromTitle_ShouldReturnCorrectTitleLength()
        {
            // Arrange
            var title = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";

            // Act
            var parsedTitle = title.PositionTitleParser();

            // Assert
            Assert.Equal(50, parsedTitle.Length);
            Assert.EndsWith("...", parsedTitle);
        }
    }
}
