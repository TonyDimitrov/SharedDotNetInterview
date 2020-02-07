namespace DotNetInterview.Web.Areas.Administration.Controllers
{
    using DotNetInterview.Common;
    using DotNetInterview.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}
