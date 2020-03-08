namespace DotNetInterview.Services
{
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public interface IFileService
    {
        Task<string> SaveFile(IFormFile file, string fileDirectory);
    }
}
