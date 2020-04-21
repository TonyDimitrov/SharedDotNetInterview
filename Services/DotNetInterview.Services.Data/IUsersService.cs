namespace DotNetInterview.Services.Data
{
    using System.Threading.Tasks;

    using DotNetInterview.Data.Models;
    using DotNetInterview.Web.ViewModels.Users;
    using DotNetInterview.Web.ViewModels.Users.DTO;

    public interface IUsersService
    {
        Task Update(ApplicationUser user, UpdateUserDTO formModel, IFileService fileService, string fileDirectory);

        DetailsUserVM Details(string userId, bool isLoggedInUser, bool isAdmin);

        Task Delete(string userId);
    }
}
