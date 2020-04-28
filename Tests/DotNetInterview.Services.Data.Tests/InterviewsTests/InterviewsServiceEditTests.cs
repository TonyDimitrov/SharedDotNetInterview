namespace DotNetInterview.Services.Data.Tests.InterviewsTests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using DotNetInterview.Data;
    using DotNetInterview.Data.Common.Repositories;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Data.Models.Enums;
    using DotNetInterview.Data.Repositories;
    using DotNetInterview.Web.ViewModels.Enums;
    using DotNetInterview.Web.ViewModels.Interviews.DTO;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Moq;

    using Xunit;

    public class InterviewsServiceEditTests
    {
        [Fact]
        public async Task EditGet_AllInterviewInformation_ReturnCorrectInformation()
        {
            // Arrange
            var interviewRepo = new Mock<IDeletableEntityRepository<Interview>>();
            var interviews = InterviewsTestData.GetInterviewsTestData();
            interviewRepo.Setup(r => r.All()).Returns(interviews);

            var importerService = new Mock<IImporterHelperService>();
            importerService.Setup(s => s.GetAllWithSelected("Bulgarian"))
                .ReturnsAsync(new List<SelectListItem>
                {
                    new SelectListItem { Value = "Bulgarian", Text = "Bulgarian", Selected = true },
                    new SelectListItem { Value = "US", Text = "US" },
                });

            var service = new InterviewsService(null, interviewRepo.Object, null, null, null, importerService.Object);

            // Act
            var interviewVM = await service.EditGet("1");

            // Asssert
            Assert.Equal("Junior with some experience", interviewVM.PositionTitle);
            Assert.Equal(PositionSeniority.JuniorDeveloper.ToString(), interviewVM.Seniority.ToString());
            Assert.Equal("Bulgarian", interviewVM.CompanyNationality);
            Assert.Equal("Bulgarian", interviewVM.CompanyListNationalities.FirstOrDefault(n => n.Selected).Value);
            Assert.Equal(EmployeesSize.Between100And1000.ToString(), interviewVM.Employees.ToString());
            Assert.Equal(LocationType.InOffice.ToString(), interviewVM.LocationType.ToString());
            Assert.Equal("Junior with some experience on .net core", interviewVM.PositionDescription);
            Assert.Equal("Sofia", interviewVM.BasedPositionLocation);
            Assert.Equal(2, interviewVM.CompanyListNationalities.Count());

            Assert.Equal("Waht is Encapsulation?", interviewVM.Questions.ToArray()[0].Content);
            Assert.Equal("Data hiding", interviewVM.Questions.ToArray()[0].GivenAnswer);
            Assert.NotNull(interviewVM.Questions.ToArray()[0].File);

            Assert.Equal("Waht is Polymorphism?", interviewVM.Questions.ToArray()[1].Content);
            Assert.Equal("Overrading methods", interviewVM.Questions.ToArray()[1].GivenAnswer);
            Assert.Null(interviewVM.Questions.ToArray()[1].File);
        }

        [Fact]
        public async Task Edit_AllInterviewInformation_ReturnCorrectInformation()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("edit_interview");

            using var dbContext = new ApplicationDbContext(options.Options);

            var interviewRepository = new EfDeletableEntityRepository<Interview>(dbContext);
            var questionRepository = new EfDeletableEntityRepository<Question>(dbContext);

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

            var service = new InterviewsService(null, interviewRepository, questionRepository, null, null, importerService.Object);
            var newInterview = InterviewsTestData.CreateInterviewTestData();
            newInterview.Questions[1].FormFile = fileMock;

            await service.Create(newInterview, "1", "fileDirectory", fileService.Object);
            var interviewId = interviewRepository.All().FirstOrDefault().Id;
            var interviewQuestionsIds = interviewRepository.All()
                .SelectMany(i => i.Questions)
                .Select(q => q.Id)
                .ToArray();
            var interviewVM = await service.EditGet(interviewId);

            interviewVM.InterviewId = interviewId;
            interviewVM.Seniority = PositionSeniorityVM.RegularDeveloper;
            interviewVM.PositionTitle = "Regular developer";
            interviewVM.PositionDescription = "3+ Years";
            interviewVM.LocationType = LocationTypeVM.InOffice;
            interviewVM.BasedPositionLocation = "Plovdiv";
            interviewVM.CompanyNationality = "German";
            interviewVM.Employees = EmployeesSizeVM.MoreThan1000;

            interviewVM.Questions[1].Content = "Interface segregation";
            interviewVM.Questions[1].GivenAnswer = "Tiny interfaces";
            interviewVM.Questions[1].Difficult = (int)QuestionRankTypeVM.MostDifficult;
            interviewVM.Questions[1].FormFile = fileMock;

            fileService.Setup(f => f.SaveFile(fileMock, "fileDirectory"))
                .ReturnsAsync("fileForInterviewQuestionChanged");

            // Act
            await service.Edit(interviewVM, "1", "fileDirectory", fileService.Object);
            var editedInterview = interviewRepository.All().FirstOrDefault(i => i.Id == interviewId);

            // Asssert
            Assert.Equal(PositionSeniority.RegularDeveloper.ToString(), editedInterview.Seniority.ToString());
            Assert.Equal("Regular developer", editedInterview.PositionTitle);
            Assert.Equal("3+ Years", editedInterview.PositionDescription);
            Assert.Equal("German", editedInterview.CompanyNationality);
            Assert.Equal(LocationType.InOffice.ToString(), editedInterview.LocationType.ToString());
            Assert.Equal("Plovdiv", editedInterview.BasedPositionLocation);
            Assert.Equal(EmployeesSize.MoreThan1000.ToString(), editedInterview.Employees.ToString());

            Assert.Equal(2, editedInterview.Questions.Count);
            Assert.Equal("Encapsulation", editedInterview.Questions.ToArray()[0].Content);
            Assert.Equal("Data hiding", editedInterview.Questions.ToArray()[0].GivenAnswer);
            Assert.Equal((int)QuestionRankTypeVM.None, (int)editedInterview.Questions.ToArray()[0].RankType);
            Assert.Null(interviewVM.Questions.ToArray()[0].File);

            Assert.Equal("Interface segregation", editedInterview.Questions.ToArray()[1].Content);
            Assert.Equal("Tiny interfaces", editedInterview.Questions.ToArray()[1].GivenAnswer);
            Assert.Equal(QuestionRankTypeVM.MostDifficult.ToString(), editedInterview.Questions.ToArray()[1].RankType.ToString());
            Assert.Equal("fileForInterviewQuestionChanged", editedInterview.Questions.ToArray()[1].UrlTask);
        }

        [Fact]
        public async Task Edit_AllInterviewInformationAndRemoveAddQuestion_ReturnCorrectQuestions()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("edit_interview2");

            using var dbContext = new ApplicationDbContext(options.Options);

            var interviewRepository = new EfDeletableEntityRepository<Interview>(dbContext);
            var questionRepository = new EfDeletableEntityRepository<Question>(dbContext);

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

            var service = new InterviewsService(null, interviewRepository, questionRepository, null, null, importerService.Object);
            var newInterview = InterviewsTestData.CreateInterviewTestData();
            newInterview.Questions[1].FormFile = fileMock;

            await service.Create(newInterview, "1", "fileDirectory", fileService.Object);
            var interviewId = interviewRepository.All().FirstOrDefault().Id;
            var interviewQuestionsIds = interviewRepository.All()
                .SelectMany(i => i.Questions)
                .Select(q => q.Id)
                .ToArray();
            var interviewVM = await service.EditGet(interviewId);

            interviewVM.InterviewId = interviewId;
            interviewVM.Seniority = PositionSeniorityVM.RegularDeveloper;
            interviewVM.PositionTitle = "Regular developer";
            interviewVM.PositionDescription = "3+ Years";
            interviewVM.LocationType = LocationTypeVM.InOffice;
            interviewVM.BasedPositionLocation = "Plovdiv";
            interviewVM.CompanyNationality = "German";
            interviewVM.Employees = EmployeesSizeVM.MoreThan1000;

            interviewVM.Questions[1].QuestionId = null;
            interviewVM.Questions[1].Content = "DI";
            interviewVM.Questions[1].GivenAnswer = "Dipendancy injection";
            interviewVM.Questions[1].Interesting = (int)QuestionRankTypeVM.MostInteresting;
            interviewVM.Questions[1].FormFile = fileMock;

            interviewVM.Questions.Add(new EditInterviewQuestionsDTO
            {
                Content = "Liskov substitution",
                GivenAnswer = "Objects of a superclass shall be replaceable with objects of its subclasses without breaking the application",
                Unexpected = (int)QuestionRankTypeVM.MostUnexpected,
                FormFile = fileMock,
            });

            fileService.Setup(f => f.SaveFile(fileMock, "fileDirectory"))
                .ReturnsAsync("fileForInterviewQuestionChanged");

            // Act
            await service.Edit(interviewVM, "1", "fileDirectory", fileService.Object);
            var editedInterview = interviewRepository.All().FirstOrDefault(i => i.Id == interviewId);

            // Asssert
            Assert.Equal(PositionSeniority.RegularDeveloper.ToString(), editedInterview.Seniority.ToString());
            Assert.Equal("Regular developer", editedInterview.PositionTitle);
            Assert.Equal("3+ Years", editedInterview.PositionDescription);
            Assert.Equal("German", editedInterview.CompanyNationality);
            Assert.Equal(LocationType.InOffice.ToString(), editedInterview.LocationType.ToString());
            Assert.Equal("Plovdiv", editedInterview.BasedPositionLocation);
            Assert.Equal(EmployeesSize.MoreThan1000.ToString(), editedInterview.Employees.ToString());

            Assert.Equal(3, editedInterview.Questions.Count);

            Assert.Equal("Encapsulation", editedInterview.Questions.ToArray()[0].Content);
            Assert.Equal("Data hiding", editedInterview.Questions.ToArray()[0].GivenAnswer);
            Assert.Equal((int)QuestionRankTypeVM.None, (int)editedInterview.Questions.ToArray()[0].RankType);
            Assert.Null(interviewVM.Questions.ToArray()[0].File);

            Assert.Equal("DI", editedInterview.Questions.ToArray()[1].Content);
            Assert.Equal("Dipendancy injection", editedInterview.Questions.ToArray()[1].GivenAnswer);
            Assert.Equal(QuestionRankTypeVM.MostInteresting.ToString(), editedInterview.Questions.ToArray()[1].RankType.ToString());
            Assert.Equal("fileForInterviewQuestionChanged", editedInterview.Questions.ToArray()[1].UrlTask);

            Assert.Equal("Liskov substitution", editedInterview.Questions.ToArray()[2].Content);
            Assert.Equal(
                "Objects of a superclass shall be replaceable with objects of its subclasses without breaking the application",
                editedInterview.Questions.ToArray()[2].GivenAnswer);
            Assert.Equal(QuestionRankTypeVM.MostUnexpected.ToString(), editedInterview.Questions.ToArray()[2].RankType.ToString());
            Assert.Equal("fileForInterviewQuestionChanged", editedInterview.Questions.ToArray()[2].UrlTask);
        }
    }
}
