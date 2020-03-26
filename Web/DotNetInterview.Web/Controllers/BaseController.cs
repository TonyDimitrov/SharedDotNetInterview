namespace DotNetInterview.Web.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Security.Claims;
    using DotNetInterview.Common;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    public class BaseController : Controller
    {
        internal string GetUserId(ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            return principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        internal bool IsAdmin() => this.User.IsInRole(GlobalConstants.AdministratorRoleName);

        internal string GetRootPath(IWebHostEnvironment hostingEnvironment, string typeFilesDirectory)
        {
            return Path.Combine(hostingEnvironment.WebRootPath, typeFilesDirectory);
        }

        internal string BuildFileContenttype(string fileName)
        {
            return Path.Combine(GlobalConstants.ImageContentType, fileName
                .Split('.', StringSplitOptions.RemoveEmptyEntries)
                .Last());
        }

        internal string FileExtension(string fileName)
        {
            return fileName
                .Split('.', StringSplitOptions.RemoveEmptyEntries)
                .Last();
        }
    }
}
