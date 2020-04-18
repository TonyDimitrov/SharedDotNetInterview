namespace DotNetInterview.Web.Controllers
{
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    using DotNetInterview.Common;
    using DotNetInterview.Web.ViewModels;
    using DotNetInterview.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly IWebHostEnvironment hostingEnvironment;

        public HomeController(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public async Task<IActionResult> Privacy()
        {
            var imagePath = this.GetRootPath(this.hostingEnvironment, "img\\");

            var filePathAndName = Path.Combine(imagePath, GlobalConstants.PolicyFileName);

            using var reader = new StreamReader(filePathAndName, Encoding.UTF8);

            var policyContent = await reader.ReadToEndAsync();

            return this.View(new PolicyVM { Content = policyContent });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorVM { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
