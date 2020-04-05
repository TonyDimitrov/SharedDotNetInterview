namespace DotNetInterview.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DotNetInterview.Common;
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

        public IEnumerable<T> GetDeletedInterviews<T>(int pageIndex)
        {
            var skipPages = (pageIndex * GlobalConstants.PagesNumber) - GlobalConstants.PagesNumber;

            return this.interviewsRepository.AllAsNoTrackingWithDeleted()
                .Where(i => i.IsDeleted)
                .OrderByDescending(i => i.DeletedOn)
                .To<T>()
                .Skip(skipPages)
                .Take(GlobalConstants.PagesNumber)
                .ToList();
        }

        public DeletedInterviewsVM GetDeletedInterviewsByPage(int pageIndex, IEnumerable<DeletedInterviewVM> interviews)
        {
            var paginationSets = (int)Math.Ceiling((double)this.interviewsRepository.AllWithDeleted()
                .Where(i => i.IsDeleted)
                .Count() / GlobalConstants.PagesNumber);

            var interviewsVM = new DeletedInterviewsVM();
            interviewsVM.DeletedInterviews = interviews;

            for (int i = GlobalConstants.PaginationLength; true; i += GlobalConstants.PaginationLength)
            {
                if (pageIndex <= i)
                {
                    if (paginationSets > i)
                    {
                        interviewsVM.StartrIndex = i - GlobalConstants.PaginationLength;
                        interviewsVM.PaginationLength = GlobalConstants.PaginationLength;
                        interviewsVM.NextDisable = string.Empty;
                    }
                    else
                    {
                        interviewsVM.StartrIndex = i - GlobalConstants.PaginationLength;
                        interviewsVM.PaginationLength = paginationSets - interviewsVM.StartrIndex;
                        interviewsVM.NextDisable = GlobalConstants.DesableLink;
                    }

                    break;
                }
                else if (paginationSets < i)
                {
                    interviewsVM.StartrIndex = i - GlobalConstants.PaginationLength;
                    interviewsVM.PaginationLength = paginationSets - interviewsVM.StartrIndex;
                    interviewsVM.NextDisable = GlobalConstants.DesableLink;

                    break;
                }
            }

            interviewsVM.CurrentSet = pageIndex;

            if (interviewsVM.StartrIndex == 0)
            {
                interviewsVM.PrevtDisable = GlobalConstants.DesableLink;
            }
            else
            {
                interviewsVM.PrevtDisable = string.Empty;
            }

            return interviewsVM;
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
