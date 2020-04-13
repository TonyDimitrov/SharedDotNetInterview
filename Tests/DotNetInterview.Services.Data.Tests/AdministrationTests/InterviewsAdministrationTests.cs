namespace DotNetInterview.Services.Data.Tests.AdministrationTests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    using DotNetInterview.Common;
    using DotNetInterview.Data;
    using DotNetInterview.Data.Common.Repositories;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Data.Models.Enums;
    using DotNetInterview.Data.Repositories;
    using DotNetInterview.Services.Data.Tests.InterviewsTests;
    using DotNetInterview.Services.Mapping;
    using DotNetInterview.Web.ViewModels;
    using DotNetInterview.Web.ViewModels.Administration.Interviews;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class InterviewsAdministrationTests
    {
        [Fact]
        public async Task GetDeletedInterviews_GetOnlyDeletedInterviews_ReturnDeleted()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("deleted_interviews");

            var interviewRepository = new EfDeletableEntityRepository<Interview>(new ApplicationDbContext(options.Options));
            var questionRepository = new EfDeletableEntityRepository<Question>(new ApplicationDbContext(options.Options));
            var commentRepository = new EfDeletableEntityRepository<Comment>(new ApplicationDbContext(options.Options));
            var likeRepository = new EfDeletableEntityRepository<Like>(new ApplicationDbContext(options.Options));
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

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

            AutoMapperConfig.RegisterMappings(typeof(ErrorVM).GetTypeInfo().Assembly);

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

            var interviewRepository = new EfDeletableEntityRepository<Interview>(new ApplicationDbContext(options.Options));
            var questionRepository = new EfDeletableEntityRepository<Question>(new ApplicationDbContext(options.Options));
            var commentRepository = new EfDeletableEntityRepository<Comment>(new ApplicationDbContext(options.Options));
            var likeRepository = new EfDeletableEntityRepository<Like>(new ApplicationDbContext(options.Options));
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

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

            AutoMapperConfig.RegisterMappings(typeof(ErrorVM).GetTypeInfo().Assembly);

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

        [Fact(Skip = "Cannot execute linq query")]
        public void GetDetailsDeletedInterview_GetOnlyDeletedInterviewsDetails_ReturnDetails()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("deleted_interviews_details");

            var interviewRepository = new EfDeletableEntityRepository<Interview>(new ApplicationDbContext(options.Options));
            var questionRepository = new EfDeletableEntityRepository<Question>(new ApplicationDbContext(options.Options));
            var commentRepository = new EfDeletableEntityRepository<Comment>(new ApplicationDbContext(options.Options));
            var likeRepository = new EfDeletableEntityRepository<Like>(new ApplicationDbContext(options.Options));
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

            var administratorService = new AdministrationService(
                interviewRepository,
                questionRepository,
                commentRepository,
                likeRepository,
                userRepository);

            var interviewsRepo = new Mock<IDeletableEntityRepository<Interview>>();
            var interviews = InterviewsTestData.GetInterviewsTestData();
            interviewsRepo.Setup(r => r.All()).Returns(interviews);

            AutoMapperConfig.RegisterMappings(typeof(ErrorVM).GetTypeInfo().Assembly);

            // Act
            var interviewVM = administratorService.GetDetailsDeletedInterview<DetailsDeletedInterviewVM>("1");

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

        [Fact(Skip = "Not implemented")]
        public void UndeleteInterview_UndeleteInterviewAndItsChildEntities_Undelete()
        {
          // TODO
        }
    }
}
