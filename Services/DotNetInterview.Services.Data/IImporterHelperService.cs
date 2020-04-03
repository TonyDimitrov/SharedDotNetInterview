namespace DotNetInterview.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface IImporterHelperService
    {
        Task<IEnumerable<SelectListItem>> GetAll();

        Task<IEnumerable<SelectListItem>> GetAllWithSelected(string selectNationality);

        Task AddNationality(string nationality);

        Task DeleteNationality(string nationality);
    }
}
