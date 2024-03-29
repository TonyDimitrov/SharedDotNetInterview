﻿namespace DotNetInterview.Services.Data.Tests.InterviewsTests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using DotNetInterview.Data;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Data.Repositories;
    using DotNetInterview.Web.ViewModels.Comments.DTO;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class InterviewsServiceDeleteTests
    {
        [Fact]
        public async Task Delete_DeleteInterviewItsQuestionsAndComments_AllDataIsDeleted()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("edit_interview1");

            using var dbContext = new ApplicationDbContext(options.Options);

            var interviewRepository = new EfDeletableEntityRepository<Interview>(dbContext);
            var questionRepository = new EfDeletableEntityRepository<Question>(dbContext);
            var commentRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            var likeRepository = new EfDeletableEntityRepository<Like>(dbContext);

            var fileService = new Mock<IFileService>();
            var fileMock = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");
            fileService.Setup(f => f.SaveFile(fileMock, "fileDirectory"))
                .ReturnsAsync("fileForInterviewQuestion");

            var nationalitiesService = new Mock<INationalitiesService>();
            nationalitiesService.Setup(s => s.GetAll())
                .ReturnsAsync(new List<SelectListItem>
                {
                    new SelectListItem { Value = "Bulgaria", Text = "Bulgaria" },
                    new SelectListItem { Value = "US", Text = "US" },
                });

            var service = new InterviewsService(
                null,
                interviewRepository,
                questionRepository,
                commentRepository,
                likeRepository,
                nationalitiesService.Object);
            var newInterview = InterviewsTestData.CreateInterviewTestData();
            newInterview.Questions[1].FormFile = fileMock;

            await service.Create(newInterview, "1", "fileDirectory", fileService.Object);
            var dbInterview = interviewRepository.All().First();
            var interviewId = dbInterview.Id;

            await service.AddComment(new AddCommentDTO { Id = interviewId, Content = "cool" }, "2");
            await service.Liked(interviewId, "1");
            await service.Liked(interviewId, "1");
            await service.Liked(interviewId, "1");
            var dbInterviewWithComment = interviewRepository.All().ToArray();

            // Act
            await service.Delete(dbInterview.Id, "1", false);
            var interviewsWithdeleted = interviewRepository.AllWithDeleted()
                .Where(i => i.IsDeleted)
                .ToArray();

            // Assert
            // Before deletion interview has one comment
            Assert.Single(dbInterviewWithComment);
            Assert.Equal(2, dbInterviewWithComment.First().Questions.Count);
            Assert.Single(dbInterviewWithComment.First().Comments);
            Assert.Single(dbInterviewWithComment.First().Likes);

            // After deletion
            Assert.False(interviewRepository.All().Any());
            Assert.Single(interviewsWithdeleted);
            Assert.Equal(2, interviewsWithdeleted.First().Questions.Count);
            Assert.Single(interviewsWithdeleted.First().Comments);
            Assert.Single(interviewsWithdeleted.First().Likes);
        }

        [Fact]
        public async Task Delete_InvalidDeleteForNotInterviewOwnerNorAdmin_AllDataIsDeleted()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("edit_interview2");

            using var dbContext = new ApplicationDbContext(options.Options);

            var interviewRepository = new EfDeletableEntityRepository<Interview>(dbContext);
            var questionRepository = new EfDeletableEntityRepository<Question>(dbContext);
            var commentRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            var likeRepository = new EfDeletableEntityRepository<Like>(dbContext);

            var fileService = new Mock<IFileService>();
            var fileMock = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");
            fileService.Setup(f => f.SaveFile(fileMock, "fileDirectory"))
                .ReturnsAsync("fileForInterviewQuestion");

            var importerService = new Mock<INationalitiesService>();
            importerService.Setup(s => s.GetAll())
                .ReturnsAsync(new List<SelectListItem>
                {
                    new SelectListItem { Value = "Bulgaria", Text = "Bulgaria" },
                    new SelectListItem { Value = "US", Text = "US" },
                });

            var service = new InterviewsService(
                null,
                interviewRepository,
                questionRepository,
                commentRepository,
                likeRepository,
                importerService.Object);
            var newInterview = InterviewsTestData.CreateInterviewTestData();
            newInterview.Questions[1].FormFile = fileMock;

            await service.Create(newInterview, "1", "fileDirectory", fileService.Object);
            var dbInterview = interviewRepository.All().First();
            var interviewId = dbInterview.Id;

            await service.AddComment(new AddCommentDTO { Id = interviewId, Content = "cool" }, "2");
            await service.Liked(interviewId, "1");
            await service.Liked(interviewId, "1");
            await service.Liked(interviewId, "1");
            var dbInterviewsWithComment = interviewRepository.All().ToArray();

            // Act
            await service.Delete(dbInterview.Id, "not owner", false);
            var interviewsAfterInvalidDeletion = interviewRepository.All().ToArray();

            // Assert
            // Before deletion interview has one comment
            Assert.Single(dbInterviewsWithComment);
            Assert.Equal(2, dbInterviewsWithComment.First().Questions.Count);
            Assert.Single(dbInterviewsWithComment.First().Comments);
            Assert.Single(dbInterviewsWithComment.First().Likes);

            // After invalid deletion interviews remain the same
            Assert.Single(interviewsAfterInvalidDeletion);
            Assert.Equal(2, interviewsAfterInvalidDeletion.First().Questions.Count);
            Assert.Single(interviewsAfterInvalidDeletion.First().Comments);
            Assert.Single(interviewsAfterInvalidDeletion.First().Likes);
        }

        [Fact]
        public async Task HardDelete_DeleteInterviewItsQuestionsAndComments_AllDataIsHardDeleted()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("edit_interview3");

            using var dbContext = new ApplicationDbContext(options.Options);

            var interviewRepository = new EfDeletableEntityRepository<Interview>(dbContext);
            var questionRepository = new EfDeletableEntityRepository<Question>(dbContext);
            var commentRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            var likeRepository = new EfDeletableEntityRepository<Like>(dbContext);

            var fileService = new Mock<IFileService>();
            var fileMock = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");
            fileService.Setup(f => f.SaveFile(fileMock, "fileDirectory"))
                .ReturnsAsync("fileForInterviewQuestion");

            var importerService = new Mock<INationalitiesService>();
            importerService.Setup(s => s.GetAll())
                .ReturnsAsync(new List<SelectListItem>
                {
                    new SelectListItem { Value = "Bulgaria", Text = "Bulgaria" },
                    new SelectListItem { Value = "US", Text = "US" },
                });

            var service = new InterviewsService(
                null,
                interviewRepository,
                questionRepository,
                commentRepository,
                likeRepository,
                importerService.Object);
            var newInterview = InterviewsTestData.CreateInterviewTestData();
            newInterview.Questions[1].FormFile = fileMock;

            await service.Create(newInterview, "1", "fileDirectory", fileService.Object);
            var dbInterview = interviewRepository.All().First();
            var interviewId = dbInterview.Id;

            await service.AddComment(new AddCommentDTO { Id = interviewId, Content = "cool" }, "2");
            await service.Liked(interviewId, "1");
            await service.Liked(interviewId, "1");
            await service.Liked(interviewId, "1");
            var dbInterviewWithComment = interviewRepository.All().ToArray();

            // Before deletion interview has one comment
            Assert.Single(dbInterviewWithComment);
            Assert.Equal(2, dbInterviewWithComment.Select(i => i.Questions.Count()).FirstOrDefault());
            Assert.Single(dbInterviewWithComment.First().Comments);
            Assert.Single(dbInterviewWithComment.First().Likes);

            // After deletion
            // Act
            await service.HardDelete(dbInterview.Id, true);
            var interviewsWithdeleted = interviewRepository.AllWithDeleted().ToArray();

            // Assert
            Assert.Empty(interviewsWithdeleted);
            Assert.Empty(questionRepository.AllWithDeleted());
            Assert.Empty(commentRepository.AllWithDeleted());
            Assert.Empty(likeRepository.AllWithDeleted());
        }

        [Fact]
        public async Task Liked_LikeOnlyOnePerUser_CorrectNumberLikes()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("edit_interview4");

            using var dbContext = new ApplicationDbContext(options.Options);

            var interviewRepository = new EfDeletableEntityRepository<Interview>(dbContext);
            var questionRepository = new EfDeletableEntityRepository<Question>(dbContext);
            var likeRepository = new EfDeletableEntityRepository<Like>(dbContext);

            var importerService = new Mock<INationalitiesService>();
            importerService.Setup(s => s.GetAll())
                .ReturnsAsync(new List<SelectListItem>
                {
                    new SelectListItem { Value = "Bulgaria", Text = "Bulgaria" },
                    new SelectListItem { Value = "US", Text = "US" },
                });

            var fileService = new Mock<IFileService>();
            var fileMock = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");
            fileService.Setup(f => f.SaveFile(fileMock, "fileDirectory"))
                .ReturnsAsync("fileForInterviewQuestion");

            var service = new InterviewsService(
                null,
                interviewRepository,
                questionRepository,
                null,
                likeRepository,
                importerService.Object);
            var newInterview = InterviewsTestData.CreateInterviewTestData();

            await service.Create(newInterview, "1", "fileDirectory", fileService.Object);
            var dbInterview = interviewRepository.All().First();
            var interviewId = dbInterview.Id;

            // Act
            await service.Liked(interviewId, "1");
            await service.Liked(interviewId, "1");
            await service.Liked(interviewId, "1");

            // Assert
            Assert.Single(likeRepository.All());
        }

        [Fact]
        public async Task Liked_LikeAndDislikePerUser_CorrectNumberLikes()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("edit_interview5");

            using var dbContext = new ApplicationDbContext(options.Options);

            var interviewRepository = new EfDeletableEntityRepository<Interview>(dbContext);
            var questionRepository = new EfDeletableEntityRepository<Question>(dbContext);
            var likeRepository = new EfDeletableEntityRepository<Like>(dbContext);

            var importerService = new Mock<INationalitiesService>();
            importerService.Setup(s => s.GetAll())
                .ReturnsAsync(new List<SelectListItem>
                {
                    new SelectListItem { Value = "Bulgaria", Text = "Bulgaria" },
                    new SelectListItem { Value = "US", Text = "US" },
                });

            var fileService = new Mock<IFileService>();
            var fileMock = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");
            fileService.Setup(f => f.SaveFile(fileMock, "fileDirectory"))
                .ReturnsAsync("fileForInterviewQuestion");

            var service = new InterviewsService(
                null,
                interviewRepository,
                questionRepository,
                null,
                likeRepository,
                importerService.Object);
            var newInterview = InterviewsTestData.CreateInterviewTestData();

            await service.Create(newInterview, "1", "fileDirectory", fileService.Object);
            var dbInterview = interviewRepository.All().First();
            var interviewId = dbInterview.Id;

            // Act
            await service.Liked(interviewId, "1");
            await service.Liked(interviewId, "1");

            // Assert
            Assert.False(likeRepository.All().First().IsLiked);
        }
    }
}
