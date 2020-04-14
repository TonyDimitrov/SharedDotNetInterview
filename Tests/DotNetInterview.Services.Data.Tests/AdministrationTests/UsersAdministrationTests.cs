namespace DotNetInterview.Services.Data.Tests.AdministrationTests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using DotNetInterview.Common;
    using DotNetInterview.Data;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Data.Repositories;
    using DotNetInterview.Services.Data.Tests.UsersTests;
    using DotNetInterview.Services.Mapping;
    using DotNetInterview.Web.ViewModels;
    using DotNetInterview.Web.ViewModels.Administration.Users;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class UsersAdministrationTests
    {
        [Fact]
        public async Task GetAllDeletedUsers_AllDeletedUsers_ReturnOnlyDeleted()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("all_deleted_users");

            using var dbContext = new ApplicationDbContext(options.Options);

            var interviewRepository = new EfDeletableEntityRepository<Interview>(dbContext);
            var questionRepository = new EfDeletableEntityRepository<Question>(dbContext);
            var commentRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            var likeRepository = new EfDeletableEntityRepository<Like>(dbContext);
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);

            var service = new AdministrationService(
                interviewRepository,
                questionRepository,
                commentRepository,
                likeRepository,
                userRepository);

            var dummyUser = UserTestData.GetUserTestData();
            dummyUser.IsDeleted = true;
            dummyUser.DeletedOn = DateTime.UtcNow;

            var dummyUser2 = UserTestData.GetUserTestData();
            dummyUser2.UserName = "dummy@dummy.com";
            dummyUser2.Email = "dummy@dummy.com";
            dummyUser2.IsDeleted = true;
            dummyUser2.DeletedOn = DateTime.UtcNow;

            var dummyUser3 = UserTestData.GetUserTestData();
            dummyUser3.UserName = "notDeleted@dummy.com";
            dummyUser3.Email = "notdeleted@dummy.com";

            await userRepository.AddAsync(dummyUser);
            await userRepository.AddAsync(dummyUser2);
            await userRepository.AddAsync(dummyUser3);
            await userRepository.SaveChangesAsync();

            AutoMapperConfig.RegisterMappings(typeof(ErrorVM).GetTypeInfo().Assembly);

            // Act
            var deletedUsers = service.GetAllDeletedUsers<DeletedUserVM>();

            // Assert
            Assert.Equal(2, deletedUsers.Count());
        }

        [Fact]
        public async Task GetDeletedUsersByPage_AllDetails_ReturnDetailsForDeletedUser()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("all_deleted_user_by_page");

            using var dbContext = new ApplicationDbContext(options.Options);

            var interviewRepository = new EfDeletableEntityRepository<Interview>(dbContext);
            var questionRepository = new EfDeletableEntityRepository<Question>(dbContext);
            var commentRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            var likeRepository = new EfDeletableEntityRepository<Like>(dbContext);
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);

            var service = new AdministrationService(
                interviewRepository,
                questionRepository,
                commentRepository,
                likeRepository,
                userRepository);

            var dummyUser = UserTestData.GetUserTestData();
            dummyUser.IsDeleted = true;
            dummyUser.DeletedOn = DateTime.UtcNow;

            var dummyUser2 = UserTestData.GetUserTestData();
            dummyUser2.UserName = "dummy@dummy.com";
            dummyUser2.Email = "dummy@dummy.com";
            dummyUser2.IsDeleted = true;
            dummyUser2.DeletedOn = DateTime.UtcNow;

            var dummyUser3 = UserTestData.GetUserTestData();
            dummyUser3.UserName = "notDeleted@dummy.com";
            dummyUser3.Email = "notdeleted@dummy.com";
            dummyUser3.IsDeleted = true;
            dummyUser3.DeletedOn = DateTime.UtcNow;

            await userRepository.AddAsync(dummyUser);
            await userRepository.AddAsync(dummyUser2);
            await userRepository.AddAsync(dummyUser3);
            await userRepository.SaveChangesAsync();

            AutoMapperConfig.RegisterMappings(typeof(ErrorVM).GetTypeInfo().Assembly);

            // Act
            var deletedUsers = service.GetAllDeletedUsers<DeletedUserVM>();
            var deletedUsersByPage = service.GetDeletedUsersByPage(1, new DeletedUsersVM(), deletedUsers);

            // Assert
            Assert.Equal(3, deletedUsersByPage.DeletedUsers.Count());
            Assert.Equal(0, deletedUsersByPage.PreviousPage);
            Assert.Equal(1, deletedUsersByPage.CurrentPage);
            Assert.Equal(1, deletedUsersByPage.PaginationLength);
            Assert.Equal(GlobalConstants.DisableLink, deletedUsersByPage.NextDisable);
            Assert.Equal(GlobalConstants.DisableLink, deletedUsersByPage.PrevtDisable);
        }

        [Fact]
        public async Task GetDetailsDeletedUser_AllDetails_ReturnDetailsForDeletedUser()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("all_deleted_user_details");

            using var dbContext = new ApplicationDbContext(options.Options);

            var interviewRepository = new EfDeletableEntityRepository<Interview>(dbContext);
            var questionRepository = new EfDeletableEntityRepository<Question>(dbContext);
            var commentRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            var likeRepository = new EfDeletableEntityRepository<Like>(dbContext);
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);

            var service = new AdministrationService(
                interviewRepository,
                questionRepository,
                commentRepository,
                likeRepository,
                userRepository);

            var dummyUser = UserTestData.GetUserTestData();
            dummyUser.IsDeleted = true;
            dummyUser.DeletedOn = DateTime.UtcNow;

            await userRepository.AddAsync(dummyUser);
            await userRepository.SaveChangesAsync();

            AutoMapperConfig.RegisterMappings(typeof(ErrorVM).GetTypeInfo().Assembly);

            // Act
            var deletedUser = service.GetAllDeletedUsers<DeletedUserVM>().FirstOrDefault();

            // Assert
            Assert.Equal("toni@toni.com", deletedUser.UserName);
            Assert.Equal("Toni Dimitrov", deletedUser.FullName);
            Assert.Equal("avatar", deletedUser.Image);
        }

        [Fact]
        public async Task UndeleteUser_UndeleteDeletedUser_ReturnUndeleted()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("undelete_user");

            using var dbContext = new ApplicationDbContext(options.Options);

            var interviewRepository = new EfDeletableEntityRepository<Interview>(dbContext);
            var questionRepository = new EfDeletableEntityRepository<Question>(dbContext);
            var commentRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            var likeRepository = new EfDeletableEntityRepository<Like>(dbContext);
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);

            var service = new AdministrationService(
                interviewRepository,
                questionRepository,
                commentRepository,
                likeRepository,
                userRepository);

            var dummyUser = UserTestData.GetUserTestData();
            dummyUser.IsDeleted = true;
            dummyUser.DeletedOn = DateTime.UtcNow;

            await userRepository.AddAsync(dummyUser);
            await userRepository.SaveChangesAsync();

            AutoMapperConfig.RegisterMappings(typeof(ErrorVM).GetTypeInfo().Assembly);
            var deletedUser = userRepository.AllWithDeleted().FirstOrDefault();
            Assert.True(deletedUser.IsDeleted);

            // Act
            await service.UndeleteUser(deletedUser.Id);
            var undeletedUser = userRepository.All().FirstOrDefault();

            // Assert
            Assert.False(undeletedUser.IsDeleted);
            Assert.Null(undeletedUser.DeletedOn);
        }
    }
}
