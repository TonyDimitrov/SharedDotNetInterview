﻿namespace DotNetInterview.Services.Data.Tests.UsersTests
{
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using DotNetInterview.Common;
    using DotNetInterview.Data;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Data.Models.Enums;
    using DotNetInterview.Data.Repositories;
    using DotNetInterview.Services.Data.Tests.InterviewsTests;
    using DotNetInterview.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class UsersDetailsTests
    {
        [Fact]
        public async Task Details_GetAllUserDetailUserIsOwner_ReturnDetails()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("details_user");

            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));
            var interviewRepository = new EfDeletableEntityRepository<Interview>(new ApplicationDbContext(options.Options));
            var mockedFile = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");
            var fileService = new Mock<IFileService>();
            fileService
                .Setup(f => f.SaveFile(mockedFile, "fileDirectory"))
                .ReturnsAsync("fileName");

            var user = UserTestData.GetUserTestData();
            user.Nationality = "Bulgarian";
            user.Position = WorkPosition.SeniorDeveloper;
            user.Description = "Experienced";

            await userRepository.AddAsync(user);
            await userRepository.SaveChangesAsync();
            var dbUserId = userRepository.AllAsNoTracking().First().Id;

            var interviewsService = new InterviewsService(null, interviewRepository, null, null, null, null);

            var interview = InterviewsTestData.CreateInterviewTestData();

            await interviewsService.Create(interview, dbUserId, "fileDirectory", fileService.Object);
            await interviewsService.Create(interview, dbUserId, "fileDirectory", fileService.Object);

            var usersService = new UsersService(userRepository);

            // Act
            var userDetails = usersService.Details<DetailsUserVM>(dbUserId, true, false);

            // Assert
            Assert.Equal("toni@toni.com", userDetails.UserName);
            Assert.Equal("Toni Dimitrov", userDetails.FullName);
            Assert.Equal("Bulgarian", userDetails.Nationality);
            Assert.Equal("Experienced", userDetails.Description);
            Assert.Equal(string.Empty, userDetails.ShowEdit);
            Assert.Equal(string.Empty, userDetails.ShowDelete);
            Assert.Equal(2, userDetails.Interviews.Count());
        }

        [Fact]
        public async Task Details_GetAllUserDetailUserIsNotOwnerNorAdmin_ReturnDetails()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("details_user2");

            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));
            var interviewRepository = new EfDeletableEntityRepository<Interview>(new ApplicationDbContext(options.Options));
            var mockedFile = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");
            var fileService = new Mock<IFileService>();
            fileService
                .Setup(f => f.SaveFile(mockedFile, "fileDirectory"))
                .ReturnsAsync("fileName");

            var user = UserTestData.GetUserTestData();
            user.Nationality = "Bulgarian";
            user.Position = WorkPosition.SeniorDeveloper;
            user.Description = "Experienced";

            await userRepository.AddAsync(user);
            await userRepository.SaveChangesAsync();
            var dbUserId = userRepository.AllAsNoTracking().First().Id;

            var interviewsService = new InterviewsService(null, interviewRepository, null, null, null, null);

            var interview = InterviewsTestData.CreateInterviewTestData();

            await interviewsService.Create(interview, dbUserId, "fileDirectory", fileService.Object);
            await interviewsService.Create(interview, dbUserId, "fileDirectory", fileService.Object);

            var usersService = new UsersService(userRepository);

            // Act
            var userDetails = usersService.Details<DetailsUserVM>(dbUserId, false, false);

            // Assert
            Assert.Equal("toni@toni.com", userDetails.UserName);
            Assert.Equal("Toni Dimitrov", userDetails.FullName);
            Assert.Equal("Bulgarian", userDetails.Nationality);
            Assert.Equal("Experienced", userDetails.Description);
            Assert.Equal(GlobalConstants.Hidden, userDetails.ShowEdit);
            Assert.Equal(GlobalConstants.Hidden, userDetails.ShowDelete);
            Assert.Equal(2, userDetails.Interviews.Count());
        }

        [Fact]
        public async Task Details_GetAllUserDetailUserIsNotOwnerButAdmin_ReturnDetails()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("details_user3");

            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));
            var interviewRepository = new EfDeletableEntityRepository<Interview>(new ApplicationDbContext(options.Options));
            var mockedFile = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");
            var fileService = new Mock<IFileService>();
            fileService
                .Setup(f => f.SaveFile(mockedFile, "fileDirectory"))
                .ReturnsAsync("fileName");

            var user = UserTestData.GetUserTestData();
            user.Nationality = "Bulgarian";
            user.Position = WorkPosition.SeniorDeveloper;
            user.Description = "Experienced";

            await userRepository.AddAsync(user);
            await userRepository.SaveChangesAsync();
            var dbUserId = userRepository.AllAsNoTracking().First().Id;

            var interviewsService = new InterviewsService(null, interviewRepository, null, null, null, null);

            var interview = InterviewsTestData.CreateInterviewTestData();

            await interviewsService.Create(interview, dbUserId, "fileDirectory", fileService.Object);
            await interviewsService.Create(interview, dbUserId, "fileDirectory", fileService.Object);

            var usersService = new UsersService(userRepository);

            // Act
            var userDetails = usersService.Details<DetailsUserVM>(dbUserId, false, true);

            // Assert
            Assert.Equal("toni@toni.com", userDetails.UserName);
            Assert.Equal("Toni Dimitrov", userDetails.FullName);
            Assert.Equal("Bulgarian", userDetails.Nationality);
            Assert.Equal("Experienced", userDetails.Description);
            Assert.Equal(GlobalConstants.Hidden, userDetails.ShowEdit);
            Assert.Equal(string.Empty, userDetails.ShowDelete);
            Assert.Equal(2, userDetails.Interviews.Count());
        }
    }
}