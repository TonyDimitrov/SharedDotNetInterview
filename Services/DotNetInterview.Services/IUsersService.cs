namespace DotNetInterview.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DotNetInterview.Data.Models;
    using DotNetInterview.Web.ViewModels.Users.DTO;

    public interface IUsersService
    {
        IEnumerable<T> GetAll<T>(int? count = null);

        T GetById<T>(string id);

        Task Updade(ApplicationUser user, UpdateUserDTO formModel, IFileService fileService, string fileDirectory);
    }
}
