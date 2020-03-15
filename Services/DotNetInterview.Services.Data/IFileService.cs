namespace DotNetInterview.Services.Data
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface IFileService
    {
        Task<string> SaveFile(IFormFile file, string fileDirectory);

        void DeleteFile(string fileDiretory, string fileName);
    }
}
