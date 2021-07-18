namespace DotNetInterview.Services.Data.Tests.UsersTests
{
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using DotNetInterview.Data;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Data.Repositories;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class UsersDeleteTests
    {
        [Fact]
        public async Task Delete_SoftDeleteUserData_DeleteUser()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("delete_user");

            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

            using var dbNationalities = new ApplicationDbContext(options.Options);
            var nationalityRepository = new NationalitiesService(dbNationalities);

            var mockedFile = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");
            var fileService = new Mock<IFileService>();
            fileService
                .Setup(f => f.SaveFile(mockedFile, "fileDirectory"))
                .ReturnsAsync("fileNameChanged");

            var user = UserTestData.GetUserTestData();

            await userRepository.AddAsync(user);
            await userRepository.SaveChangesAsync();
            var dbUserId = userRepository.AllAsNoTracking().First().Id;

            var usersService = new UsersService(userRepository, nationalityRepository);

            // Act
            await usersService.Delete(dbUserId);

            // Assert
            Assert.Null(userRepository.All().FirstOrDefault(u => u.Id == dbUserId));
        }
    }
}
