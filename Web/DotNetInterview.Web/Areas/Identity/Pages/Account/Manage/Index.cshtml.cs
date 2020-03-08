﻿namespace DotNetInterview.Web.Areas.Identity.Pages.Account.Manage
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using DotNetInterview.Data.Models;
    using DotNetInterview.Services;
    using DotNetInterview.Web.ViewModels.Enums;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using DotNetInterview.Web.ViewModels.Users.DTO;
    using Microsoft.AspNetCore.Hosting;
    using System.IO;
    using DotNetInterview.Common;

    public partial class IndexModel : PageModel
    {
        private const string NoDefineNationality = "Not selected";

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IUsersService usersService;
        private readonly IFileService fileService;
        private readonly IImporterHelperService importerHelperService;
        private readonly IWebHostEnvironment webHostEnvironment;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUsersService usersService,
            IFileService fileService,
            IImporterHelperService importerHelperService,
            IWebHostEnvironment webHostEnvironment
            )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.usersService = usersService;
            this.fileService = fileService;
            this.importerHelperService = importerHelperService;
            this.webHostEnvironment = webHostEnvironment;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "First name")]
            [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
            public string FirstName { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Last name")]
            [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
            public string LastName { get; set; }

            [Display(Name = "Date of birth")]
            public DateTime? DateOfBirth { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Nationality")]
            public string Nationality { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Description")]
            [StringLength(1000, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
            public string Description { get; set; }

            public IEnumerable<SelectListItem> Nationalities { get; set; }

            [Display(Name = "Position")]
            public PositionSeniorityVM Position { get; set; }

            [Display(Name = "Avatar")]
            public string Image { get; set; }

            [Display(Name = "Avatar")]
            public IFormFile FormFile { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await this.userManager.GetUserNameAsync(user);
            var phoneNumber = await this.userManager.GetPhoneNumberAsync(user);
            var appUser = await this.userManager.GetUserAsync(this.User);
            this.Username = userName;

            this.Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                DateOfBirth = appUser.DateOfBirth,
                Description = appUser.Description,
                Nationality = appUser?.Nationality ?? NoDefineNationality,
                Nationalities = this.importerHelperService.GetAll<IEnumerable<string>>()
                .Select(n =>
                {
                    if (n == this.Input.Nationality)
                    {
                        return new SelectListItem(n, n, true);
                    }
                    else
                    {
                        return new SelectListItem(n, n, false);
                    }
                }),
                Position = Enum.Parse<PositionSeniorityVM>(appUser.Position.ToString()),
                Image = appUser?.Image,
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            await this.LoadAsync(user);
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            if (!this.ModelState.IsValid)
            {
                await this.LoadAsync(user);
                return this.Page();
            }

            var phoneNumber = await this.userManager.GetPhoneNumberAsync(user);

            if (this.Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await this.userManager.SetPhoneNumberAsync(user, this.Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await this.userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

            var updateUserDTO = new UpdateUserDTO
            {
                LastName = this.Input.LastName,
                Nationality = this.Input.Nationality,
                Position = this.Input.Position,
                Description = this.Input.Description,
                Image = this.Input.FormFile,
            };

            await this.usersService.Updade(
                                        user,
                                        updateUserDTO,
                                        this.fileService,
                                        this.GetRootPath(
                                            this.webHostEnvironment,
                                            GlobalConstants.ImageFilesDirectory));

            await this.signInManager.RefreshSignInAsync(user);
            this.StatusMessage = "Your profile has been updated";
            return this.RedirectToPage();
        }

        internal string GetRootPath(IWebHostEnvironment hostingEnvironment, string typeFilesDirectory)
        {
            return Path.Combine(hostingEnvironment.WebRootPath, typeFilesDirectory);
        }
    }
}
