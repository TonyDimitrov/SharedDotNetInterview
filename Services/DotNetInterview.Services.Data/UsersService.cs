﻿namespace DotNetInterview.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using DotNetInterview.Common;
    using DotNetInterview.Data.Common.Repositories;
    using DotNetInterview.Data.Models;
    using DotNetInterview.Data.Models.Enums;
    using DotNetInterview.Services.Data.Extensions;
    using DotNetInterview.Web.ViewModels.Users;
    using DotNetInterview.Web.ViewModels.Users.DTO;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;

        public UsersService(IDeletableEntityRepository<ApplicationUser> userRepository)
        {
            this.userRepository = userRepository;
        }

        public DetailsUserVM Details(string userId, bool isLoggedInUser, bool isAdmin)
        {
            var userDTO = this.userRepository
                .All()
                .Where(u => u.Id == userId)
                .Select(u => new DetailsUserDTO
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Nationality = u.Nationality,
                    Position = u.Position.ToString(),
                    Description = u.Description,
                    MemberSince = u.CreatedOn,
                    DateOfBirth = u.DateOfBirth,
                    Image = u.Image,
                    Interviews = u.Interviews
                        .OrderByDescending(i => i.CreatedOn)
                        .Select(i => new DetailsUserInterviewsVM
                        {
                            InterviewId = i.Id,
                            Title = i.PositionTitle.PositionTitleParser(),
                            Seniority = i.Seniority.ToString(),
                            Date = i.CreatedOn.ToString(GlobalConstants.FormatDate),
                            Likes = i.Likes
                            .Where(l => l.IsLiked)
                            .Count(),
                            Qns = i.Questions.Count,
                        })
                        .ToList(),
                })
                .FirstOrDefault();

            if (userDTO == null)
            {
                return null;
            }

            return new DetailsUserVM
            {
                Id = userDTO.Id,
                UserName = userDTO.UserName,
                FullName = $"{userDTO.FirstName} {userDTO.LastName}",
                Position = userDTO.Position,
                Nationality = userDTO.Nationality != null ? userDTO.Nationality : GlobalConstants.NoInformation,
                MemberSince = userDTO.MemberSince.ToString(GlobalConstants.FormatDate),
                DateOfBirth = userDTO.DateOfBirth != null ? userDTO.DateOfBirth?.ToString(GlobalConstants.FormatDateShort) : GlobalConstants.NoInformation,
                Description = userDTO.Description != null ? userDTO.Description : GlobalConstants.NoInformation,
                Image = userDTO.Image,
                ShowEdit = isLoggedInUser ? string.Empty : GlobalConstants.Hidden,
                ShowDelete = isLoggedInUser || isAdmin ? string.Empty : GlobalConstants.Hidden,
                Interviews = userDTO.Interviews,
            };
        }

        public async Task Update(ApplicationUser user, UpdateUserDTO formModel, IFileService fileService, string fileDirectory)
        {
            user.FirstName = formModel.FirstName;
            user.LastName = formModel.LastName;
            user.Nationality = formModel.Nationality;
            user.Position = Enum.Parse<WorkPosition>(formModel.Position.ToString());
            user.DateOfBirth = formModel.DateOfBirth;
            user.Description = formModel.Description;

            var savedFileName = await fileService.SaveFile(formModel.Image, fileDirectory);

            if (user.Image != null && !user.Image.Contains(GlobalConstants.DefaultFilePart))
            {
                fileService.DeleteFile(fileDirectory, user.Image);
            }

            user.Image = savedFileName;

            this.userRepository.Update(user);
            await this.userRepository.SaveChangesAsync();
        }

        public async Task Delete(string userId)
        {
            var user = await this.userRepository.GetByIdWithDeletedAsync(userId);

            if (user == null)
            {
                return;
            }

            this.userRepository.Delete(user);

            await this.userRepository.SaveChangesAsync();
        }
    }
}
