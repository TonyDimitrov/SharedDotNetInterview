namespace DotNetInterview.Services.Data.Tests.ExtensionTests
{
    using System;

    using DotNetInterview.Services.Data.Extensions;
    using Xunit;

    public class DateTimeExtensionTests
    {
        [Fact]
        public void Format_DateTime_ShouldFormatAndReturnToLocalTime()
        {
            // Arrange
            var date = new DateTime(2015, 05, 15, 12, 16, 26, DateTimeKind.Utc);
            var hourToLocal = date.ToLocalTime().Hour;

            // Act
            var formattedDate = date.DateTimeViewFormater();

            // Assert
            Assert.Equal($"15 May 2015 {hourToLocal:00}:16", formattedDate);
        }
    }
}
