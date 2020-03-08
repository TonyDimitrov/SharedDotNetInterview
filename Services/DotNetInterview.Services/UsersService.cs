namespace DotNetInterview.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DotNetInterview.Common;
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

        public async Task Updade(ApplicationUser user, UpdateUserDTO formModel, IFileService fileService, string fileDirectory)
        {
            user.LastName = formModel.LastName;
            user.Nationality = formModel.Nationality;
            user.Position = Enum.Parse<WorkPosition>(formModel.Position.ToString());
            user.Description = formModel.Description;

            var savedFileName = await fileService.SaveFile(formModel.Image, fileDirectory);

            if (user.Image != null && !user.Image.Contains(GlobalConstants.DefaultFilePart))
            {
                fileService.DeleteFile(fileDirectory, user.Image);
            }

            user.Image = savedFileName;

            this.categoriesRepository.Update(user);
            await this.categoriesRepository.SaveChangesAsync();
        }
    }
}
