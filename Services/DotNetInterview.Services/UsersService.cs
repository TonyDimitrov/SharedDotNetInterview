namespace DotNetInterview.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DotNetInterview.Common;
    using DotNetInterview.Data.Common.Repositories;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Data.Models.Enums;
    using DotNetInterview.Web.ViewModels.Enums;
    using DotNetInterview.Web.ViewModels.Users;
    using DotNetInterview.Web.ViewModels.Users.DTO;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> categoriesRepository;

        public UsersService(IDeletableEntityRepository<ApplicationUser> categoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
        }

        public T Details<T>(string userId)
        {
            var userDTO = this.categoriesRepository
                .All()
                .Where(u => u.Id == userId)
                .Select(u => new DetailsUserDTO
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Nationality = u.Nationality,
                    Position = Enum.Parse<PositionSeniorityVM>(u.Position.ToString()),
                    Description = u.Description,
                    MemberSince = u.CreatedOn,
                    DateOfBirth = u.DateOfBirth,
                    Image = u.Image,
                    Interviews = u.Interviews
                        .Select(i => new DetailsUserInterviewsVM
                        {
                            InterviewId = i.Id,
                            Title = i.PositionTitle,
                            Seniority = Enum.Parse<PositionSeniorityVM>(i.Seniority.ToString()),
                            Date = i.CreatedOn.ToString(GlobalConstants.FormatDate),
                            Likes = i.Likes,
                            Qns = i.Questions.Count,
                        })
                        .ToList(),
                })
                .FirstOrDefault();

            return (T)(object)new DetailsUserVM
            {
                FullName = $"{userDTO.FirstName} {userDTO.LastName}",
                Position = userDTO.Position,
                Nationality = userDTO.Nationality,
                MemberSince = userDTO.MemberSince,
                DateOfBirth = userDTO.DateOfBirth,
                Description = userDTO.Description,
                Image = userDTO.Image,
                Interviews = userDTO.Interviews,
            };
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
