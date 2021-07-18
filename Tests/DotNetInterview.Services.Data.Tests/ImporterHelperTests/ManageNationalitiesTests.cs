namespace DotNetInterview.Services.Data.Tests.ImporterHelperTests
{
    using System.Linq;
    using System.Threading.Tasks;

    using DotNetInterview.Data;
    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class ManageNationalitiesTests
    {
        [Fact]
        public async Task AddNationality_AddNationalityRecord_Added()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase("add_nationalities");

            using var dbNationalities = new ApplicationDbContext(options.Options);

            var service = new NationalitiesService(dbNationalities);

            // Act
            await service.AddNationality("Bulgaria");
            var storedNationality = await service.GetAll();

            // Assert
            Assert.Single(storedNationality);
            Assert.Equal("Bulgaria", storedNationality.First().Text);
        }

        [Fact]
        public async Task DeleteNationality_DeleteNationalityRecord_Deleted()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase("delete_nationalities");

            using var dbNationalities = new ApplicationDbContext(options.Options);

            var service = new NationalitiesService(dbNationalities);
            await service.AddNationality("Bulgaria");

            // Act
            await service.DeleteNationality(1);
            var storedNationality = await service.GetAll();

            // Assert
            Assert.Empty(storedNationality);
        }

        [Fact]
        public async Task GetAllWithSelected_DeleteNationalityRecord_Deleted()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase("getAll_nationalities");

            using var dbNationalities = new ApplicationDbContext(options.Options);

            var service = new NationalitiesService(dbNationalities);

            var selectedNationality = "Bulgaria";

            await service.AddNationality(selectedNationality);
            await service.AddNationality("US");
            await service.AddNationality("English");

            // Act
            var storedNationality = await service.GetAllWithSelected(1);

            // Assert
            Assert.Equal(3, storedNationality.Count());
            Assert.True(storedNationality.First(n => n.Text == selectedNationality && n.Value == "1").Selected);
        }
    }
}
