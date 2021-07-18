namespace DotNetInterview.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DotNetInterview.Data.Models;
    using DotNetInterview.Web.ViewModels.Common;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface INationalitiesService
    {
        Task<Nationality> GetById(int id);

        Task<IEnumerable<SelectListItem>> GetAll();

        Task<IEnumerable<SelectListItem>> GetAllWithSelected(int? selectNationality);

        Task<DbOperation> AddNationality(string nationality);

        Task<DbOperation> DeleteNationality(int nationalityId);
    }
}
