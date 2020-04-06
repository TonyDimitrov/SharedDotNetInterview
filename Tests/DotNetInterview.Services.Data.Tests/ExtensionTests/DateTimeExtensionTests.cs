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
            var hour = 12;
            var date = new DateTime(2015, 05, 15, hour, 16, 26, DateTimeKind.Utc);
            var localDifferentHoursFromUtc = DateTime.Now.Hour - DateTime.UtcNow.Hour;

            var formattedHour = hour + localDifferentHoursFromUtc;

            // Act
            var formattedDate = date.DateTimeViewFormater();

            // Assert
            Assert.Equal($"15 May 2015 {formattedHour:00}:16", formattedDate);
        }
    }
}
