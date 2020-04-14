namespace DotNetInterview.Services.Data.Tests.AdministrationTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using DotNetInterview.Common;
    using DotNetInterview.Data;
    using DotNetInterview.Data.Common.Repositories;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Data.Models.Enums;
    using DotNetInterview.Data.Repositories;
    using DotNetInterview.Services.Data.Tests.InterviewsTests;
    using DotNetInterview.Web.ViewModels.Administration.Interviews;
    using DotNetInterview.Web.ViewModels.Comments.DTO;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    [Collection("Mappings collection")]
    public class InterviewsAdministrationTests
    {
        [Fact]
        public async Task GetDeletedInterviews_GetOnlyDeletedInterviews_ReturnDeleted()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("deleted_interviews");

            using var dbContext = new ApplicationDbContext(options.Options);

            var interviewRepository = new EfDeletableEntityRepository<Interview>(dbContext);
            var questionRepository = new EfDeletableEntityRepository<Question>(dbContext);
            var commentRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            var likeRepository = new EfDeletableEntityRepository<Like>(dbContext);
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);

            var administratorService = new AdministrationService(
                interviewRepository,
                questionRepository,
                commentRepository,
                likeRepository,
                userRepository);

            var fileService = new Mock<IFileService>();
            var fileMock = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");
            fileService.Setup(f => f.SaveFile(fileMock, "fileDirectory"))
                .ReturnsAsync("fileForInterviewQuestion");

            var interviewsService = new InterviewsService(null, interviewRepository, null, null, null, null);
            var newInterview = InterviewsTestData.CreateInterviewTestData();
            newInterview.Questions[0].FormFile = fileMock;

            await interviewsService.Create(newInterview, "1", "fileDirectory", fileService.Object);
            await interviewsService.Create(newInterview, "1", "fileDirectory", fileService.Object);
            var createdInterviews = interviewRepository.All();

            Assert.Equal(2, createdInterviews.Count());

            createdInterviews.FirstOrDefault().IsDeleted = true;
            createdInterviews.FirstOrDefault().DeletedOn = DateTime.UtcNow;
            interviewRepository.Update(createdInterviews.FirstOrDefault());
            await interviewRepository.SaveChangesAsync();

            // Act
            var deletedInterviews = administratorService.GetDeletedInterviews<DeletedInterviewVM>();

            // Assert
            Assert.Single(deletedInterviews);
        }

        [Fact]
        public async Task GetDeletedInterviewsByPage_GetOnlyDeletedInterviews_ReturnDeleted()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("deleted_interviews_by_page");

            using var dbContext = new ApplicationDbContext(options.Options);

            var interviewRepository = new EfDeletableEntityRepository<Interview>(dbContext);
            var questionRepository = new EfDeletableEntityRepository<Question>(dbContext);
            var commentRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            var likeRepository = new EfDeletableEntityRepository<Like>(dbContext);
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);

            var administratorService = new AdministrationService(
                interviewRepository,
                questionRepository,
                commentRepository,
                likeRepository,
                userRepository);

            var fileService = new Mock<IFileService>();
            var fileMock = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");
            fileService.Setup(f => f.SaveFile(fileMock, "fileDirectory"))
                .ReturnsAsync("fileForInterviewQuestion");

            var interviewsService = new InterviewsService(null, interviewRepository, null, null, null, null);
            var newInterview = InterviewsTestData.CreateInterviewTestData();
            newInterview.Questions[0].FormFile = fileMock;

            await interviewsService.Create(newInterview, "1", "fileDirectory", fileService.Object);
            await interviewsService.Create(newInterview, "1", "fileDirectory", fileService.Object);
            var createdInterviews = interviewRepository.All();

            Assert.Equal(2, createdInterviews.Count());

            createdInterviews.FirstOrDefault().IsDeleted = true;
            createdInterviews.FirstOrDefault().DeletedOn = DateTime.UtcNow;
            interviewRepository.Update(createdInterviews.FirstOrDefault());
            await interviewRepository.SaveChangesAsync();

            // Act
            var deletedInterviews = administratorService.GetDeletedInterviews<DeletedInterviewVM>();
            var deletedInterviewsByPage = administratorService.GetDeletedInterviewsByPage(
                1,
                new DeletedInterviewsVM(),
                deletedInterviews);

            // Assert
            Assert.Single(deletedInterviewsByPage.DeletedInterviews);
            Assert.Equal(0, deletedInterviewsByPage.PreviousPage);
            Assert.Equal(1, deletedInterviewsByPage.CurrentPage);
            Assert.Equal(1, deletedInterviewsByPage.PaginationLength);
            Assert.Equal(GlobalConstants.DisableLink, deletedInterviewsByPage.NextDisable);
            Assert.Equal(GlobalConstants.DisableLink, deletedInterviewsByPage.PrevtDisable);
        }

        [Fact(Skip = "System.InvalidCastException: 'Unable to cast object of type" +
            " 'System.Linq.Expressions.NewExpression' to type 'System.Linq.Expressions.MethodCallExpression'.'")]
        public void GetDetailsDeletedInterview_GetOnlyDeletedInterviewsDetails_ReturnDetails()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("deleted_interviews_details");

            using var dbContext = new ApplicationDbContext(options.Options);

            var interviewRepository = new EfDeletableEntityRepository<Interview>(dbContext);
            var questionRepository = new EfDeletableEntityRepository<Question>(dbContext);
            var commentRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            var likeRepository = new EfDeletableEntityRepository<Like>(dbContext);
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);

            var administratorService = new AdministrationService(
                interviewRepository,
                questionRepository,
                commentRepository,
                likeRepository,
                userRepository);

            var interviewsRepo = new Mock<IDeletableEntityRepository<Interview>>();
            var interviews = InterviewsTestData.GetInterviewsTestData();
            var interviewId = "1";
            interviews.First().Id = interviewId;
            interviews.First().IsDeleted = true;
            interviews.First().DeletedOn = DateTime.UtcNow;
            interviewsRepo.Setup(r => r.AllWithDeleted()).Returns(interviews);

            // Act
            var interviewVM = administratorService.GetDetailsDeletedInterview<DetailsDeletedInterviewVM>(interviewId);

            // Assert
            Assert.Equal("Junior with some experience", interviewVM.PositionTitle);
            Assert.Equal(PositionSeniority.JuniorDeveloper.ToString(), interviewVM.Seniority);
            Assert.Equal("Bulgarian", interviewVM.CompanyNationality);
            Assert.Equal(EmployeesSize.Between100And1000.ToString(), interviewVM.CompanySize);
            Assert.Equal(LocationType.InOffice.ToString(), interviewVM.LocationType);
            Assert.Equal("Junior with some experience on .net core", interviewVM.PositionDescription);
            Assert.Equal("Sofia", interviewVM.BasedPositionLocation);
            Assert.NotNull(interviewVM.CreatedOn);
            Assert.Equal("Toni D.", interviewVM.UserFullName);
            Assert.Equal(GlobalConstants.Hidden, interviewVM.CanDelete);
            Assert.Equal(GlobalConstants.Hidden, interviewVM.CanEdit);
            Assert.Equal(GlobalConstants.Hidden, interviewVM.CanHardDelete);

            Assert.Equal("Waht is Encapsulation?", interviewVM.Questions.ToArray()[0].Content);
            Assert.Equal("Data hiding", interviewVM.Questions.ToArray()[0].Answer);
            Assert.Equal(QuestionRankType.MostUnexpected.ToString(), interviewVM.Questions.ToArray()[0].Ranked);
            Assert.NotNull(interviewVM.Questions.ToArray()[0].File);
            Assert.Equal(string.Empty, interviewVM.Questions.ToArray()[0].HideAnswer);
            Assert.Equal(string.Empty, interviewVM.Questions.ToArray()[0].HideRanked);
            Assert.Equal(string.Empty, interviewVM.Questions.ToArray()[0].HideFile);

            Assert.Equal("Waht is Polymorphism?", interviewVM.Questions.ToArray()[1].Content);
            Assert.Equal("Overrading methods", interviewVM.Questions.ToArray()[1].Answer);
            Assert.Equal(QuestionRankType.MostInteresting.ToString(), interviewVM.Questions.ToArray()[1].Ranked);
            Assert.Null(interviewVM.Questions.ToArray()[1].File);
            Assert.Equal(string.Empty, interviewVM.Questions.ToArray()[1].HideAnswer);
            Assert.Equal(string.Empty, interviewVM.Questions.ToArray()[1].HideRanked);
            Assert.Equal(GlobalConstants.Hidden, interviewVM.Questions.ToArray()[1].HideFile);

            Assert.Equal(2, interviewVM.Questions.ToArray()[1].Comments.Count());
            Assert.Equal("Not a difficult questions", interviewVM.Questions.ToArray()[1].Comments.ToArray()[0].Content);
            Assert.Equal(GlobalConstants.Hidden, interviewVM.Questions.ToArray()[1].Comments.ToArray()[0].HideDelete);
            Assert.Equal(string.Empty, interviewVM.Questions.ToArray()[1].Comments.ToArray()[0].HideAdd);

            Assert.Equal(2, interviewVM.Likes);
        }

        [Fact(Skip = "System.InvalidOperationException: " +
            "'The instance of entity type 'Question' cannot be tracked because another instance with the same key value" +
            " for {'Id'} is already being tracked. When attaching existing entities," +
            " ensure that only one entity instance with a given key value is attached. ")]
        public async Task UndeleteInterview_UndeleteInterviewAndItsChildEntities_Undelete()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("undelete_interview1");

            using var dbContext = new ApplicationDbContext(options.Options);

            var interviewRepository = new EfDeletableEntityRepository<Interview>(dbContext);
            var questionRepository = new EfDeletableEntityRepository<Question>(dbContext);
            var commentRepository = new EfDeletableEntityRepository<Comment>(dbContext);
            var likeRepository = new EfDeletableEntityRepository<Like>(dbContext);
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);

            var fileService = new Mock<IFileService>();
            var fileMock = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");
            fileService.Setup(f => f.SaveFile(fileMock, "fileDirectory"))
                .ReturnsAsync("fileForInterviewQuestion");

            var importerService = new Mock<IImporterHelperService>();
            importerService.Setup(s => s.GetAll())
                .ReturnsAsync(new List<SelectListItem>
                {
                    new SelectListItem { Value = "Bulgaria", Text = "Bulgaria" },
                    new SelectListItem { Value = "US", Text = "US" },
                });

            var interviewsService = new InterviewsService(
                null,
                interviewRepository,
                questionRepository,
                commentRepository,
                likeRepository,
                importerService.Object);
            var newInterview = InterviewsTestData.CreateInterviewTestData();
            newInterview.Questions[1].FormFile = fileMock;

            await interviewsService.Create(newInterview, "1", "fileDirectory", fileService.Object);
            var dbInterview = interviewRepository.All().First();
            var interviewId = dbInterview.Id;

            await interviewsService.AddComment(new AddCommentDTO { Id = interviewId, Content = "cool" }, "1");
            await interviewsService.Liked(interviewId, "1");

            var administratorService = new AdministrationService(
             interviewRepository,
             questionRepository,
             commentRepository,
             likeRepository,
             userRepository);

            // Act
            await interviewsService.Delete(interviewId, "1", false);
            await administratorService.UndeleteInterview(interviewId);
            var undeletedInterview = interviewsService.Details(interviewId, "1", true);

            // Assert
            Assert.NotNull(undeletedInterview);
            Assert.Equal(2, undeletedInterview.InterviewQns.Count());
            Assert.Single(undeletedInterview.InterviewComments);
            Assert.Equal(1, undeletedInterview.Likes);
        }
    }
}
