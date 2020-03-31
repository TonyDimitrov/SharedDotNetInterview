namespace DotNetInterview.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DotNetInterview.Data;
    using DotNetInterview.Data.Common.Repositories;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Services.Mapping;
    using DotNetInterview.Web.ViewModels.Administration.Interviews;
    using DotNetInterview.Web.ViewModels.Administration.Nationalities;
    using DotNetInterview.Web.ViewModels.Administration.Users;
    using Microsoft.EntityFrameworkCore;

    public class AdministrationService : IAdministrationService
    {
        private readonly IDeletableEntityRepository<Interview> interviewsRepository;
        private readonly ApplicationDbContext db;

        public AdministrationService(
            IDeletableEntityRepository<Interview> interviewsRepository,
            ApplicationDbContext db)
        {
            this.interviewsRepository = interviewsRepository;
            this.db = db;
        }

        public DeletedUsersVM GetAllDeletedUsers()
        {
            return null;
        }

        public IEnumerable<T> GetDeletedInterviews<T>()
        {
            return this.interviewsRepository.AllAsNoTrackingWithDeleted()
                .Where(i => i.IsDeleted)
                .To<T>()
                .ToList();
        }

        public T GetDetailsDeletedInterview<T>(string interviewId)
        {
            // map properties
            return this.interviewsRepository.AllAsNoTrackingWithDeleted()
                .Where(i => i.IsDeleted && i.Id == interviewId)
                .To<T>()
                .FirstOrDefault();
        }

        public Task UndeleteInterview(string interviewId)
        {
            throw new NotImplementedException();
        }

        public ManageNationalitiesVM GetNationalities()
        {
            throw new NotImplementedException();
        }

        public DeletedInterviewsVM GetDeletedInterviews()
        {
            throw new NotImplementedException();
        }
    }
}
