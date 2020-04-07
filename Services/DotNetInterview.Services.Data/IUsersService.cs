namespace DotNetInterview.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DotNetInterview.Data.Models;
    using DotNetInterview.Web.ViewModels.Users.DTO;

    public interface IUsersService
    {
        Task Updade(ApplicationUser user, UpdateUserDTO formModel, IFileService fileService, string fileDirectory);

        T Details<T>(string userId, bool isLoggedInUser, bool isAdmin);

        Task Delete(string userId);
    }
}
