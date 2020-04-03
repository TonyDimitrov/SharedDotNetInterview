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
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public AdministrationService(
            IDeletableEntityRepository<Interview> interviewsRepository,
            IDeletableEntityRepository<Question> questionRepository,
            IDeletableEntityRepository<Comment> commentsRepository,
            IDeletableEntityRepository<Like> likesRepository,
            IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.interviewsRepository = interviewsRepository;
            this.questionsRepository = questionRepository;
            this.commentsRepository = commentsRepository;
            this.likesRepository = likesRepository;
            this.usersRepository = usersRepository;
        }

        public IEnumerable<T> GetAllDeletedUsers<T>()
        {
            return this.usersRepository.AllAsNoTrackingWithDeleted()
                .Where(u => u.IsDeleted)
                .OrderByDescending(u => u.DeletedOn)
                .To<T>()
                .ToList();
        }

        public T GetDetailsDeletedUser<T>(string userId)
        {
            return this.usersRepository.AllWithDeleted()
                .Where(u => u.Id == userId)
                .To<T>()
                .FirstOrDefault();
        }

        public async Task UndeleteUser(string userId)
        {
            var deletedUser = await this.usersRepository.GetByIdWithDeletedAsync(userId);

            this.usersRepository.Undelete(deletedUser);

            await this.usersRepository.SaveChangesAsync();
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
