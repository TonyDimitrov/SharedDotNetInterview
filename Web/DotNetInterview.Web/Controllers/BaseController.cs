namespace DotNetInterview.Web.Controllers
{
    using System;
    using System.IO;
    using System.Security.Claims;

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

        internal string GetRootPath(IWebHostEnvironment hostingEnvironment, string typeFilesDirectory)
        {
            return Path.Combine(hostingEnvironment.WebRootPath, typeFilesDirectory);
        }
    }
}
