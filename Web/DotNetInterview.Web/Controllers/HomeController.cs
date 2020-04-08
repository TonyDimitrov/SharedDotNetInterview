namespace DotNetInterview.Web.Controllers
{
    using System.Diagnostics;

    using DotNetInterview.Web.ViewModels;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorVM { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
