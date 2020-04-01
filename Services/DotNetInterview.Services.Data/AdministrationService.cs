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
        private readonly IDeletableEntityRepository<Question> questionsRepository;
        private readonly IDeletableEntityRepository<Comment> commentsRepository;
        private readonly IDeletableEntityRepository<Like> likesRepository;
        private readonly ApplicationDbContext db;

        public AdministrationService(
            IDeletableEntityRepository<Interview> interviewsRepository,
            IDeletableEntityRepository<Question> questionEntityRepository,
            IDeletableEntityRepository<Comment> commentsEntityRepository,
            IDeletableEntityRepository<Like> likesRepository,
            ApplicationDbContext db)
        {
            this.interviewsRepository = interviewsRepository;
            this.questionsRepository = questionEntityRepository;
            this.commentsRepository = commentsEntityRepository;
            this.likesRepository = likesRepository;
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
                .OrderByDescending(i => i.DeletedOn)
                .To<T>()
                .ToList();
        }

        public T GetDetailsDeletedInterview<T>(string interviewId)
        {
            return this.interviewsRepository.AllAsNoTrackingWithDeleted()
                .Where(i => i.IsDeleted && i.Id == interviewId)
                .OrderByDescending(i => i.DeletedOn)
                .To<T>()
                .FirstOrDefault();
        }

        public async Task UndeleteInterview(string interviewId)
        {
            var dbInterview = this.interviewsRepository.AllAsNoTrackingWithDeleted()
            .Include(i => i.Comments)
            .Include(i => i.Likes)
            .Include(i => i.Questions)
            .ThenInclude(q => q.Comments)
            .FirstOrDefault(i => i.Id == interviewId);

            if (dbInterview != null)
            {
                foreach (var q in dbInterview.Questions)
                {
                    foreach (var c in q.Comments)
                    {
                        this.commentsRepository.Undelete(c);
                    }
                }

                await this.commentsRepository.SaveChangesAsync();

                foreach (var q in dbInterview.Questions)
                {
                    this.questionsRepository.Undelete(q);
                }

                await this.questionsRepository.SaveChangesAsync();

                foreach (var c in dbInterview.Comments)
                {
                    this.commentsRepository.Undelete(c);
                }

                await this.commentsRepository.SaveChangesAsync();

                foreach (var l in dbInterview.Likes)
                {
                    this.likesRepository.Undelete(l);
                }

                await this.likesRepository.SaveChangesAsync();

                this.interviewsRepository.Undelete(dbInterview);

                await this.interviewsRepository.SaveChangesAsync();
            }
        }

        public ManageNationalitiesVM GetNationalities()
        {
            throw new NotImplementedException();
        }
    }
}
