﻿namespace DotNetInterview.Services.Data.Tests.CommentsTests
{
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using DotNetInterview.Data;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Data.Repositories;
    using DotNetInterview.Services.Data.Tests.InterviewsTests;
    using DotNetInterview.Services.Data.Tests.UsersTests;
    using DotNetInterview.Web.ViewModels.Comments.DTO;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class CommentsServiceDeleteTests
    {
        [Fact]
        public async Task Delete_DeleteCommentFromInterviewIfInterviewOwner_ReturnNoComment()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("delete_comments1");

            using var dbContext = new ApplicationDbContext(options.Options);

            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var interviewRepository = new EfDeletableEntityRepository<Interview>(dbContext);
            var questionRepository = new EfDeletableEntityRepository<Question>(dbContext);
            var commentRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            var mockedFile = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");
            var fileService = new Mock<IFileService>();
            fileService
                .Setup(f => f.SaveFile(mockedFile, "fileDirectory"))
                .ReturnsAsync("fileName");

            var nationalitiesService = new Mock<INationalitiesService>();

            nationalitiesService.Setup(s => s.GetById(3))
                .ReturnsAsync(new Nationality { Id = 3, CompanyNationality = "German" });

            var user = UserTestData.GetUserTestData();
            await userRepository.AddAsync(user);
            await userRepository.SaveChangesAsync();
            var dbUserId = userRepository.AllAsNoTracking().First().Id;

            var interviewsService = new InterviewsService(null, interviewRepository, questionRepository, commentRepository, null, nationalitiesService.Object);

            var interview = InterviewsTestData.CreateInterviewTestData();

            await interviewsService.Create(interview, dbUserId, "fileDirectory", fileService.Object);

            var interviewId = interviewRepository.All().First().Id;

            var comment = new AddCommentDTO
            {
                Id = interviewId,
                Content = "comment",
            };

            await interviewsService.AddComment(comment, dbUserId);

            var comments = interviewRepository.All().SelectMany(i => i.Comments).ToList();
            Assert.Single(comments);

            var commentId = comments.First().Id;
            var commentFromRepo = commentRepository.All().First(c => c.Id == commentId);
            Assert.Equal("comment", commentFromRepo.Content);

            var commentsService = new CommentsService(commentRepository);

            // Act
            await commentsService.Delete(commentId, dbUserId, false);

            // Assert
            Assert.Null(commentRepository.All().FirstOrDefault());
        }

        [Fact]
        public async Task Delete_DeleteCommentIfNotOwnerButAdmin_ReturnNoComment()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("delete_comments2");

            var nationalitiesService = new Mock<INationalitiesService>();

            using var dbContext = new ApplicationDbContext(options.Options);

            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var interviewRepository = new EfDeletableEntityRepository<Interview>(dbContext);
            var questionRepository = new EfDeletableEntityRepository<Question>(dbContext);
            var commentRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            var mockedFile = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");
            var fileService = new Mock<IFileService>();
            fileService
                .Setup(f => f.SaveFile(mockedFile, "fileDirectory"))
                .ReturnsAsync("fileName");
            using var dbNationalities = new ApplicationDbContext(options.Options);
            var nationalityService = new NationalitiesService(dbNationalities);

            var user = UserTestData.GetUserTestData();
            await userRepository.AddAsync(user);
            await userRepository.SaveChangesAsync();
            var dbUserId = userRepository.AllAsNoTracking().First().Id;

            var interviewsService = new InterviewsService(null, interviewRepository, questionRepository, commentRepository, null, nationalityService);

            var interview = InterviewsTestData.CreateInterviewTestData();

            await interviewsService.Create(interview, dbUserId, "fileDirectory", fileService.Object);

            var interviewId = interviewRepository.All().First().Id;

            var comment = new AddCommentDTO
            {
                Id = interviewId,
                Content = "comment",
            };

            await interviewsService.AddComment(comment, dbUserId);

            var comments = interviewRepository.All().SelectMany(i => i.Comments).ToList();
            Assert.Single(comments);

            var commentId = comments.First().Id;
            var commentFromRepo = commentRepository.All().First(c => c.Id == commentId);
            Assert.Equal("comment", commentFromRepo.Content);

            var commentsService = new CommentsService(commentRepository);

            // Act
            await commentsService.Delete(commentId, "other", true);

            // Assert
            Assert.Null(commentRepository.All().FirstOrDefault());
        }

        [Fact]
        public async Task Delete_CannotDeleteCommentIfNotOwnerNorAdmin_ReturnComment()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("delete_comments3");

            using var dbContext = new ApplicationDbContext(options.Options);

            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var interviewRepository = new EfDeletableEntityRepository<Interview>(dbContext);
            var questionRepository = new EfDeletableEntityRepository<Question>(dbContext);
            var commentRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            var mockedFile = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");
            var fileService = new Mock<IFileService>();
            fileService
                .Setup(f => f.SaveFile(mockedFile, "fileDirectory"))
                .ReturnsAsync("fileName");
            using var dbNationalities = new ApplicationDbContext(options.Options);
            var nationalityService = new NationalitiesService(dbNationalities);

            var user = UserTestData.GetUserTestData();
            await userRepository.AddAsync(user);
            await userRepository.SaveChangesAsync();
            var dbUserId = userRepository.AllAsNoTracking().First().Id;

            var interviewsService = new InterviewsService(null, interviewRepository, questionRepository, commentRepository, null, nationalityService);

            var interview = InterviewsTestData.CreateInterviewTestData();

            await interviewsService.Create(interview, dbUserId, "fileDirectory", fileService.Object);

            var interviewId = interviewRepository.All().First().Id;

            var comment = new AddCommentDTO
            {
                Id = interviewId,
                Content = "comment",
            };

            await interviewsService.AddComment(comment, dbUserId);

            var comments = interviewRepository.All().SelectMany(i => i.Comments).ToList();
            Assert.Single(comments);

            var commentId = comments.First().Id;
            var commentFromRepo = commentRepository.All().First(c => c.Id == commentId);
            Assert.Equal("comment", commentFromRepo.Content);

            var commentsService = new CommentsService(commentRepository);

            // Act
            await commentsService.Delete(commentId, "other", false);

            // Assert
            Assert.Single(commentRepository.All());
        }
    }
}
