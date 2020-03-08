namespace DotNetInterview.Services
{
    using DotNetInterview.Data.Models;
    using DotNetInterview.Web.ViewModels.Users.DTO;
    using System.Collections.Generic;

    public interface IUsersService
    {
        IEnumerable<T> GetAll<T>(int? count = null);

        T GetById<T>(string id);

        void Updade(ApplicationUser user, UpdateUserDTO formModel);
    }
}
