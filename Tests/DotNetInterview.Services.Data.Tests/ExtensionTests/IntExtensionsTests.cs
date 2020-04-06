namespace DotNetInterview.Services.Data.Tests.ExtensionTests
{
    using System;
    using System.Collections.Generic;

    using DotNetInterview.Services.Data.Extensions;
    using Xunit;

    public class IntExtensionsTests
    {
        private IReadOnlyDictionary<int, string> seniorities = new Dictionary<int, string>
            {
                { 0, "Not specified" },
                { 1, "Junior developer" },
                { 2, "Regular developer" },
                { 3, "Senior developer" },
                { 4, "Lead developer" },
                { 5, "Architect" },
                { 99, "Other" },
            };

        [Fact]
        public void ParseNumber_ToSeniority_ShouldReturnCorrectSeniority()
        {
            // Arrange
            foreach (var seniority in this.seniorities)
            {
                // Act
                var returnedSeniority = seniority.Key.SeniorityNameParser();

                // Assert
                Assert.Equal(returnedSeniority, seniority.Value);
            }
        }

        [Fact]
        public void ParseNonExistingNumber_ToSeniority_ShouldThrowException()
        {
            // Arrange
            var nonExistingSeniority = int.MaxValue;

            // Act
            var ex = Assert.Throws<ArgumentException>(() => nonExistingSeniority.SeniorityNameParser());

            // Assert
            Assert.Equal($"Seniority type [{nonExistingSeniority}] is invalid!", ex.Message);
        }
    }
}
