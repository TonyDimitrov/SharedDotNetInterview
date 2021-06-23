namespace DotNetInterview.Services.Data.Tests.UsersTests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using DotNetInterview.Data;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Data.Models.Enums;
    using DotNetInterview.Data.Repositories;
    using DotNetInterview.Services.Data.Tests.InterviewsTests;
    using DotNetInterview.Web.ViewModels.Enums;
    using DotNetInterview.Web.ViewModels.Users;
    using DotNetInterview.Web.ViewModels.Users.DTO;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class UsersUpdateTests
    {
        [Fact]
        public async Task Updade_UpdateAllUserProperties_ReturnUpdatedData()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("update_user");

            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

            using var dbNationalities = new ApplicationDbContext(options.Options);
            var nationalityRepository = new NationalitiesService(dbNationalities);

            var mockedFile = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");
            var fileService = new Mock<IFileService>();
            fileService
                .Setup(f => f.SaveFile(mockedFile, "fileDirectory"))
                .ReturnsAsync("fileNameChanged");

            var user = UserTestData.GetUserTestData();
            user.UserNationality = "Bulgarian";
            user.Position = WorkPosition.SeniorDeveloper;
            user.Description = "Experienced";

            await userRepository.AddAsync(user);
            await userRepository.SaveChangesAsync();
            var dbUserId = userRepository.AllAsNoTracking().First().Id;

            var usersService = new UsersService(userRepository, nationalityRepository);
            var updateUser = new UpdateUserDTO
            {
                FirstName = "Antony",
                LastName = "O`Sallivan",
                Position = PersonSeniorityVM.TechnicalArchitect,
                Nationality = "English",
                Description = "Very",
                DateOfBirth = DateTime.UtcNow,
                Image = mockedFile,
            };

            // Act
            await usersService.Update(user, updateUser, fileService.Object, "fileDirectory");
            var userDetails = usersService.Details(dbUserId, true, false);

            // Assert
            Assert.Equal("toni@toni.com", userDetails.UserName);
            Assert.Equal("Antony O`Sallivan", userDetails.FullName);
            Assert.Equal("English", userDetails.Nationality);
            Assert.Equal("Very", userDetails.Description);
            Assert.NotNull(userDetails.DateOfBirth);
            Assert.Equal("fileNameChanged", userDetails.Image);
        }
    }
}
