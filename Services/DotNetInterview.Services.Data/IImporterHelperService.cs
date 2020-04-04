namespace DotNetInterview.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DotNetInterview.Web.ViewModels.Common;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface IImporterHelperService
    {
        Task<IEnumerable<SelectListItem>> GetAll();

        Task<IEnumerable<SelectListItem>> GetAllWithSelected(string selectNationality);

        Task<DbOperation> AddNationality(string nationality);

        Task<DbOperation> DeleteNationality(string nationality);
    }
}
