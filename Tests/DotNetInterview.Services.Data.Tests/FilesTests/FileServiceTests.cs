namespace DotNetInterview.Services.Data.Tests.FilesTests
{
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using DotNetInterview.Data;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Data.Repositories;
    using DotNetInterview.Services.Data.Tests.InterviewsTests;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
    using Microsoft.EntityFrameworkCore;
    using Moq;

    using Xunit;

    public class FileServiceTests
    {
        [Fact]
        public async Task SaveFile_SaveFileFromFormAndReturnName_ReturnName()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("save_file");

            using var dbContext = new ApplicationDbContext(options.Options);

            var interviewRepository = new EfDeletableEntityRepository<Interview>(dbContext);
            var questionRepository = new EfDeletableEntityRepository<Question>(dbContext);

            var fileService = new Mock<IFileService>();
            var fileMock = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");
            fileService.Setup(f => f.SaveFile(fileMock, "fileDirectory"))
                .ReturnsAsync("fileName");
            using var dbNationalities = new ApplicationDbContext(options.Options);
            var nationalityService = new NationalitiesService(dbNationalities);

            var interviewService = new InterviewsService(null, interviewRepository, questionRepository, null, null, nationalityService);

            var newInterview = InterviewsTestData.CreateInterviewTestData();
            newInterview.Questions[0].FormFile = fileMock;

            // Act
            await interviewService.Create(newInterview, "1", "fileDirectory", fileService.Object);
            var fileName = interviewRepository.All().FirstOrDefault().Questions.FirstOrDefault().UrlTask;

            // Arrange
            Assert.Equal("fileName", fileName);
        }

        [Fact]
        public async Task DeleteFile_SaveFileFromFormAndReturnName_ReturnName()
        {
            // Arrange
            var fullPath = Path.GetFullPath("base");
            var directory = Directory.CreateDirectory(fullPath);
            var fileName = Path.Combine(directory.FullName, "text.txt");
            await File.WriteAllTextAsync(fileName, "some content");
            var fileService = new FileService();

            var exists = File.Exists(fileName);
            Assert.True(exists);

            // Act
            fileService.DeleteFile(directory.FullName, "text.txt");

            // Arrange
            exists = File.Exists(fileName);
            Assert.False(exists);
        }
    }
}
