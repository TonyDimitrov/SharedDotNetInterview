namespace DotNetInterview.Services
{
    using System;
    using System.Collections.Generic;

    using DotNetInterview.Data.Common.Repositories;
    using DotNetInterview.Data.Models;

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

        public void Updade<T>(T user)
        {
            throw new NotImplementedException();
        }
    }
}
