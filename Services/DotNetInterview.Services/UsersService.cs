namespace DotNetInterview.Services
{
    using System;
    using System.Collections.Generic;

    using DotNetInterview.Data.Common.Repositories;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Data.Models.Enums;
    using DotNetInterview.Web.ViewModels.Users.DTO;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> categoriesRepository;

        public UsersService(IDeletableEntityRepository<ApplicationUser> categoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
        }

        public IEnumerable<T> GetAll<T>(int? count = null)
        {
            throw new NotImplementedException();
        }

        public T GetById<T>(string id)
        {
            throw new NotImplementedException();
        }

        public void Updade(ApplicationUser user, UpdateUserDTO formModel)
        {
            user.LastName = formModel.LastName;
            user.Nationality = formModel.Nationality;
            user.Position = Enum.Parse<WorkPosition>(formModel.Position.ToString());
            user.Image = formModel.Image;
            this.categoriesRepository.Update(user);
        }
    }
}
